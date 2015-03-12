using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlOptimization.ExtensionMethods
{
    /// <summary>
    /// Contains static methods for bitmap manipulation
    /// </summary>
    public static class ImageUtils
    {

        #region Enums

        /// <summary>
        /// Location for starting point when cropping bitmap for square shape
        /// </summary>
        public enum CropPosition
        {
            LeftOrTop,
            RightOrBottom,
            Center
        }

        #endregion

        #region Image manipulation static methods
        /// <summary>
        /// Creates grayscale image from original bitmap
        /// </summary>
        /// <param name="SourceBitmap">Bitmap that needs to be converted to grayscale</param>
        /// <returns>Grayscale copy of input image</returns>
        public static Bitmap MakeGrayscale(this Bitmap SourceBitmap)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(SourceBitmap.Width, SourceBitmap.Height);
            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);
            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][] 
      {
         new float[] {.3f, .3f, .3f, 0, 0},
         new float[] {.59f, .59f, .59f, 0, 0},
         new float[] {.11f, .11f, .11f, 0, 0},
         new float[] {0, 0, 0, 1, 0},
         new float[] {0, 0, 0, 0, 1}
      });
            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();
            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);
            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(SourceBitmap, new Rectangle(0, 0, SourceBitmap.Width, SourceBitmap.Height),
               0, 0, SourceBitmap.Width, SourceBitmap.Height, GraphicsUnit.Pixel, attributes);
            //dispose the Graphics object
            g.Dispose();
            return newBitmap;
        }

        /// <summary>
        /// Draws vertical lines on the bitmap (usefull for watermark protection)
        /// </summary>
        /// <param name="SourceBitmap"></param>
        /// <param name="LineSpacing"></param>
        /// <param name="LineColor"></param>
        /// <param name="LineStyle"></param>
        /// <returns></returns>
        public static Bitmap DrawVerticalLines(this Bitmap SourceBitmap, int LineSpacing, string LineColor, System.Drawing.Drawing2D.DashStyle LineStyle = DashStyle.Dot)
        {
            Graphics _graph = Graphics.FromImage(SourceBitmap);
            int _position = LineSpacing;
            while (_position < SourceBitmap.Width)
            {
                Pen _dotpen = new Pen(GetColorFromHex(LineColor));
                _dotpen.DashStyle = LineStyle;
                _graph.DrawLine(_dotpen, new Point(_position, 0), new Point(_position, SourceBitmap.Height));
                _position += LineSpacing;
            }
            return SourceBitmap;
        }

        /// <summary>
        /// Draws horizontal lines on the bitmap (usefull for watermark protection)
        /// </summary>
        /// <param name="SourceBitmap"></param>
        /// <param name="LineSpacing"></param>
        /// <param name="LineColor"></param>
        /// <param name="LineStyle"></param>
        /// <returns></returns>
        public static Bitmap DrawHorizontalLines(this Bitmap SourceBitmap, int LineSpacing, string LineColor, System.Drawing.Drawing2D.DashStyle LineStyle = DashStyle.Dot)
        {
            Graphics _graph = Graphics.FromImage(SourceBitmap);
            int _position = LineSpacing;
            while (_position < SourceBitmap.Width)
            {
                Pen _dotpen = new Pen(GetColorFromHex(LineColor));
                _dotpen.DashStyle = LineStyle;
                _graph.SmoothingMode = SmoothingMode.HighQuality;
                _graph.DrawLine(_dotpen, new Point(0, _position), new Point(SourceBitmap.Width, _position));
                _position += LineSpacing;
            }
            return SourceBitmap;
        }

        /// <summary>
        /// Draws diagonal lines on the bitmap (usefull for watermark protection)
        /// </summary>
        /// <param name="SourceBitmap"></param>
        /// <param name="LineColor"></param>
        /// <param name="LineStyle"></param>
        /// <returns></returns>
        public static Bitmap DrawDiagonalLines(this Bitmap SourceBitmap, string LineColor, System.Drawing.Drawing2D.DashStyle LineStyle = DashStyle.Solid)
        {
            Graphics _photo = Graphics.FromImage(SourceBitmap);
            _photo.SmoothingMode = SmoothingMode.HighQuality;
            Pen _dotpen = new Pen(GetColorFromHex(LineColor));
            _dotpen.DashStyle = LineStyle;
            _photo.DrawLine(new Pen(GetColorFromHex(LineColor)), 0, 0, SourceBitmap.Width, SourceBitmap.Height);
            _photo.DrawLine(new Pen(GetColorFromHex(LineColor)), 0, SourceBitmap.Height, SourceBitmap.Width, 0);
            return SourceBitmap;
        }

        /// <summary>
        /// Draws centered text on the bitmap (watermark protection)
        /// </summary>
        /// <param name="SourceBitmap"></param>
        /// <param name="WatermarkText"></param>
        /// <param name="WatermarkFont"></param>
        /// <returns></returns>
        public static Bitmap DrawTextWatermark(this Bitmap SourceBitmap, string WatermarkText, string WatermarkFont)
        {
            int _width = SourceBitmap.Width;
            int _height = SourceBitmap.Height;
            int _fontSize = 1;
            Graphics _photo = Graphics.FromImage(SourceBitmap);
            _photo.SmoothingMode = SmoothingMode.HighQuality;
            Font _font = new Font(WatermarkFont, 1, FontStyle.Bold); ;
            SizeF _size = _photo.MeasureString(WatermarkText, _font);
            while (_size.Width < SourceBitmap.Width - 2)
            {
                _font = new Font(WatermarkFont, _fontSize, FontStyle.Bold);
                _size = _photo.MeasureString(WatermarkText, _font);
                _fontSize++;
            }
            int yPixlesFromBottom = 0;
            float yPosFromBottom = ((_height - yPixlesFromBottom) - (_size.Height / 2));
            float xCenterOfImg = (_width / 2);
            float yCenterOfImg = (_height / 2);
            yCenterOfImg -= _font.GetHeight() / 2;
            StringFormat StrFormat = new StringFormat();
            StrFormat.Alignment = StringAlignment.Center;
            SolidBrush semiTransBrush2 = new SolidBrush(Color.FromArgb(100, 0, 0, 0));
            _photo.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            _photo.DrawString(WatermarkText,
                _font,
                semiTransBrush2,
                new PointF(xCenterOfImg + 1, yCenterOfImg + 1),
                StrFormat);
            SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(100, 255, 255, 255));
            _photo.DrawString(WatermarkText,
                _font,
                semiTransBrush,
                new PointF(xCenterOfImg, yCenterOfImg),
                StrFormat);
            Graphics grWatermark = Graphics.FromImage(SourceBitmap);
            ImageAttributes imageAttributes = new ImageAttributes();
            ColorMap colorMap = new ColorMap();
            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
            ColorMap[] remapTable = { colorMap };
            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);
            float[][] colorMatrixElements = {
										new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
										new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
										new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
										new float[] {0.0f,  0.0f,  0.0f,  0.3f, 0.0f},
										new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}};
            ColorMatrix wmColorMatrix = new ColorMatrix(colorMatrixElements);
            imageAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default,
                ColorAdjustType.Bitmap);
            _photo.Dispose();
            grWatermark.Dispose();
            return SourceBitmap;
        }

        /// <summary>
        /// Returns bitmap resized and cropped so that both height and width are the same
        /// </summary>
        /// <param name="SourceBitmap">Bitmap you want to scale to square</param>
        /// <param name="SideSize">Size of result bitmap in pixels</param>
        /// <param name="CropMode">What start point to take when cropping resized image to square</param>
        /// <returns>Sqare scaled and cropped bitmap</returns>
        public static Bitmap ResizeToSquare(this Bitmap SourceBitmap, int SideSize, CropPosition CropMode = CropPosition.LeftOrTop)
        {
            Bitmap _WorkingBitmap;
            Rectangle _Rect;
            int _WidthCropPosition;
            int _HeightCropPosition;
            _WidthCropPosition = 0;
            _HeightCropPosition = 0;
            if (SourceBitmap.Height < SourceBitmap.Width)
            {
                _WorkingBitmap = ResizeByHeight(SourceBitmap, SideSize);
                switch (CropMode)
                {
                    case CropPosition.Center:
                        _WidthCropPosition = (_WorkingBitmap.Width - SideSize) / 2;
                        break;
                    case CropPosition.LeftOrTop:
                        _WidthCropPosition = 0;
                        break;
                    case CropPosition.RightOrBottom:
                        _WidthCropPosition = _WorkingBitmap.Width - SideSize;
                        break;
                    default:
                        _WidthCropPosition = 0;
                        break;
                }
            }
            else
            {
                _WorkingBitmap = ResizeByWidth(SourceBitmap, SideSize);
                switch (CropMode)
                {
                    case CropPosition.Center:
                        _HeightCropPosition = (_WorkingBitmap.Height - SideSize) / 2;
                        break;
                    case CropPosition.LeftOrTop:
                        _HeightCropPosition = 0;
                        break;
                    case CropPosition.RightOrBottom:
                        _HeightCropPosition = _WorkingBitmap.Height - SideSize;
                        break;
                    default:
                        _HeightCropPosition = 0;
                        break;
                }
            }
            _Rect = new Rectangle(_WidthCropPosition, _HeightCropPosition, SideSize, SideSize);
            _WorkingBitmap = _WorkingBitmap.Clone(_Rect, _WorkingBitmap.PixelFormat);
            return _WorkingBitmap;
        }

        /// <summary>
        /// Returns bitmap resized so that bitmap can fit to square of size represented by SideSize parameter.
        /// </summary>
        /// <param name="SourceBitmap">Bitmap you want to scale to fit square</param>
        /// <param name="SideSize">Size of square side you want bitmap to fit in</param>
        /// <returns>Scaled bitmap</returns>
        public static Bitmap ResizeToFitSquare(this Bitmap SourceBitmap, int SideSize)
        {
            if (SourceBitmap.Height > SourceBitmap.Width)
            {
                return ResizeByHeight(SourceBitmap, SideSize);
            }
            else
            {
                return ResizeByWidth(SourceBitmap, SideSize);
            }
        }

        /// <summary>
        /// Returns bitmap scaled by height
        /// </summary>
        /// <param name="SourceBitmap">Bitmap you want to scale by height</param>
        /// <param name="Height">Height of the result bitmap</param>
        /// <returns>Bitmap scaled by height</returns>
        public static Bitmap ResizeByHeight(this Bitmap SourceBitmap, int Height)
        {
            Bitmap _ResultBitmap;
            Graphics _Graphic;
            int _ResultWidth;
            double _Ration;
            _Ration = ((double)Height / (double)SourceBitmap.Height) * (double)100;
            _ResultWidth = (int)Math.Round(SourceBitmap.Width * (_Ration / 100));
            _ResultBitmap = new Bitmap(_ResultWidth, Height);
            _Graphic = Graphics.FromImage((System.Drawing.Image)_ResultBitmap);
            _Graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
            _Graphic.DrawImage(SourceBitmap, 0, 0, _ResultWidth, Height);
            _Graphic.Dispose();
            return _ResultBitmap;
        }

        /// <summary>
        /// Returns bitmap scaled by height
        /// </summary>
        /// <param name="SourceBitmap">Bitmap you want to scale by width</param>
        /// <param name="Height">Height of the result bitmap</param>
        /// <returns>Bitmap scaled by width</returns>
        public static Bitmap ResizeByWidth(this Bitmap SourceBitmap, int Width)
        {
            Bitmap _ResultBitmap;
            Graphics _Graphic;
            int _ResultHeight;
            double _Ration;
            _Ration = ((double)Width / (double)SourceBitmap.Width) * (double)100;
            _ResultHeight = (int)Math.Round(SourceBitmap.Height * (_Ration / 100));
            _ResultBitmap = new Bitmap(Width, _ResultHeight);
            _Graphic = Graphics.FromImage((System.Drawing.Image)_ResultBitmap);
            _Graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
            _Graphic.DrawImage(SourceBitmap, 0, 0, Width, _ResultHeight);
            _Graphic.Dispose();
            return _ResultBitmap;
        }

        /// <summary>
        /// Retuns base64 encoded string for bitmap. Usage in web <img src="data:image/png;base64,iVBORw0KGgoAAAANS..." />
        /// </summary>
        /// <param name="SourceBitmap">Bitmap you want to convert to base64 string</param>
        /// <param name="Format">Format of the encoded bitmap</param>
        /// <returns>base64 string from bitmap</returns>
        public static string BitmapToBase64(this Bitmap SourceBitmap, System.Drawing.Imaging.ImageFormat Format)
        {
            using (MemoryStream _MemStr = new MemoryStream())
            {
                SourceBitmap.Save(_MemStr, Format);
                byte[] imageBytes = _MemStr.ToArray();
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        /// <summary>
        /// Retuns image from base64 string
        /// </summary>
        /// <param name="base64String">base64 string encoded bitmap</param>
        /// <returns>Image decoded from base64 string</returns>
        public static Image Base64ToBitmap(this string base64String)
        {
            byte[] _imageBytes = Convert.FromBase64String(base64String);
            MemoryStream _MemStr = new MemoryStream(_imageBytes, 0, _imageBytes.Length);
            _MemStr.Write(_imageBytes, 0, _imageBytes.Length);
            Image _resultBitmap = Image.FromStream(_MemStr, true);
            return _resultBitmap;
        }

        /// <summary>
        /// Returns image from base64 string
        /// </summary>
        /// <param name="SourceBitmap">Bitmap on which you want to apply rounded corners</param>
        /// <param name="Radius">Radius of bitmap corners</param>
        /// <returns>Rounded corner bitmap</returns>
        public static Bitmap RoundedCornerImage(this Bitmap SourceBitmap, int Radius)
        {
            Bitmap _DestBitmap = new Bitmap(SourceBitmap.Width, SourceBitmap.Height);
            Graphics _Graph = Graphics.FromImage(_DestBitmap);
            _Graph.Clear(Color.Transparent);
            _Graph.SmoothingMode = (System.Drawing.Drawing2D.SmoothingMode.AntiAlias);
            Brush _brush = new System.Drawing.TextureBrush(SourceBitmap);
            Rectangle _destRect = new Rectangle(0, 0, SourceBitmap.Width, SourceBitmap.Height);
            System.Drawing.Drawing2D.GraphicsPath _GraphPath = new System.Drawing.Drawing2D.GraphicsPath();
            _GraphPath.AddArc(_destRect.X, _destRect.Y, Radius, Radius, 180, 90);
            _GraphPath.AddArc(_destRect.X + _destRect.Width - Radius, _destRect.Y, Radius, Radius, 270, 90);
            _GraphPath.AddArc(_destRect.X + _destRect.Width - Radius, _destRect.Y + _destRect.Height - Radius, Radius, Radius, 0, 90);
            _GraphPath.AddArc(_destRect.X, _destRect.Y + _destRect.Height - Radius, Radius, Radius, 90, 90);
            _Graph.FillPath(_brush, _GraphPath);
            _Graph.Dispose();
            return _DestBitmap;
        }


        /// <summary>
        /// Rotates source bitmap in a direction sent by RotateType parameter
        /// </summary>
        /// <param name="SourceBitmap"></param>
        /// <param name="RotateType"></param>
        /// <returns></returns>
        public static Bitmap RotateBitmap(this Bitmap SourceBitmap, RotateFlipType RotateType)
        {
            Bitmap _DestBitmap = (Bitmap)SourceBitmap.Clone();
            _DestBitmap.RotateFlip(RotateType);
            return _DestBitmap;
        }


        #endregion

        #region Common static methods
        /// <summary>
        /// .Net Color instance from html hex string
        /// </summary>
        /// <param name="HexString">Html hex color string e.g #FF0000</param>
        /// <returns>.Net Color instance from html hex string</returns>
        public static Color GetColorFromHex(string HexString)
        {
            return System.Drawing.ColorTranslator.FromHtml(HexString);
        }
        #endregion
    }
}
