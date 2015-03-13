using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using HtmlOptimization.ExtensionMethods;
using System.Configuration;
using HtmlOptimization.Config.Sections;

namespace HtmlOptimization.HtmlHelpers
{
    public static class Images
    {
        public static MD5 md5 = MD5.Create();

        static Config.Sections.ConfigSection config = ConfigurationManager.GetSection("htmlOptimization") as ConfigSection;

        public static Config.Sections.ConfigSection Config
        {
            get
            {
                return config;
            }
        }

        public static IHtmlString HtmlImageThumbnail(string imagePath, int width, int height, string alt = null, string @class = null)
        {
            return HtmlImageThumbnail(imagePath, new Size(width, height), alt, @class);
        }

        public static IHtmlString HtmlImageThumbnail(string imagePath, Size size, string alt = null, string @class = null)
        {
            var cacheFolder = !string.IsNullOrWhiteSpace( Config.HtmlImageThumbnail.CacheFolder) ? Config.HtmlImageThumbnail.CacheFolder : "/ThumbnailCache";
            var context = HttpContext.Current;
            var path = context.Server.MapPath(imagePath);
            var hashName = string.Concat(GetMd5Hash(string.Concat(path.Trim().ToLower(), "-", size.Width, "x", size.Height)), Path.GetExtension(imagePath).ToLower());
            var cachedImagePath = context.Server.MapPath(Path.Combine(cacheFolder, hashName));

            if (!Directory.Exists(context.Server.MapPath(cacheFolder)) && Config.HtmlImageThumbnail.CreateCacheFolder)
            {
                Directory.CreateDirectory(context.Server.MapPath(cacheFolder));
            }

            if (!File.Exists(cachedImagePath) && File.Exists(path))
            {
                using (Bitmap bmp = new Bitmap(path))
                {
                    if (size.Width > size.Height)
                    {
                        bmp.ResizeByWidth(size.Width).Save(cachedImagePath);
                    }
                    else if (size.Height > size.Width)
                    {
                        bmp.ResizeByHeight(size.Height).Save(cachedImagePath);
                    }
                    else
                    {
                        bmp.ResizeToSquare(size.Height, ImageUtils.CropPosition.Center).Save(cachedImagePath);
                    }
                }
            }

            var src = string.Concat(cacheFolder, hashName);

            XElement imgTag = new XElement("img", new XAttribute("src", src), new XAttribute("alt", !String.IsNullOrWhiteSpace(alt) ? alt : string.Empty), new XAttribute("class", !String.IsNullOrWhiteSpace(@class) ? @class : string.Empty));

            return new HtmlString(imgTag.ToString());
        }

        private static string GetMd5Hash(string input)
        {
            byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}
