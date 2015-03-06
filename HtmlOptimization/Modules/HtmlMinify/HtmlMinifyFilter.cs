using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HtmlOptimization.Modules.HtmlMinify
{
    public class HtmlMinifyFilter : Stream
    {
        public HtmlMinifyFilter(HttpContext context)
		{
			this.Sink = context.Response.Filter;
		}

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override void Flush()
        {
            this.Sink.Flush();
        }

        public override long Length
        {
            get { return 0; }
        }

        public override long Position
        {
            get;
            set;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return this.Sink.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return this.Sink.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            this.Sink.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            string html = Encoding.UTF8.GetString(buffer, offset, count);
            html = new HtmlOptimization.Modules.HtmlMinify.HtmlContentCompressor().Compress(html);
            byte[] outdata = System.Text.Encoding.UTF8.GetBytes(html);
            this.Sink.Write(outdata, 0, outdata.GetLength(0));

        }

        public override void Close()
        {
            this.Sink.Close();
        }



        protected Stream Sink { get; set; }
    }
}
