using HtmlOptimization.Config;
using HtmlOptimization.Config.Elements;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Web;

namespace HtmlOptimization.Modules
{
    public class CompressModule : ModuleBase, IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.PreRequestHandlerExecute += context_PreRequestHandlerExecute;
        }

        void context_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            HttpApplication app = sender as HttpApplication;
            string acceptEncoding = app.Request.Headers["Accept-Encoding"];
            Stream prevUncompressedStream = app.Response.Filter;
            string contentType = app.Context.Response.ContentType;

            if (app.Context.Request.HttpMethod == "GET" &&
                contentType.Equals("text/html") &&
                app.Context.Response.StatusCode == 200 &&
                app.Context.CurrentHandler != null &&
                string.IsNullOrWhiteSpace(app.Context.Request.CurrentExecutionFilePathExtension) ? true : ProcessExtension(app.Context.Request.CurrentExecutionFilePathExtension))
            {
                if (acceptEncoding == null || acceptEncoding.Length == 0)
                    return;

                acceptEncoding = acceptEncoding.ToLower();

                if (acceptEncoding.Contains("deflate") || acceptEncoding == "*")
                {
                    // deflate
                    if (GetExtensionCompressinType(app.Context.Request.CurrentExecutionFilePathExtension).HasFlag(CompressionType.Deflate))
                    {
                        app.Response.Filter = new DeflateStream(prevUncompressedStream,
                            CompressionMode.Compress);
                        app.Response.AppendHeader("Content-Encoding", "deflate");
                    }

                }
                else if (acceptEncoding.Contains("gzip"))
                {
                    // gzip
                    if (GetExtensionCompressinType(app.Context.Request.CurrentExecutionFilePathExtension).HasFlag(CompressionType.GZip))
                    {
                        app.Response.Filter = new GZipStream(prevUncompressedStream,
                            CompressionMode.Compress);
                        app.Response.AppendHeader("Content-Encoding", "gzip");
                    }
                }
            }
        }

        public void Dispose()
        {

        }

        public override bool ProcessExtension(string extension)
        {
            if (string.IsNullOrWhiteSpace(extension))
            {
                return true;
            }
            else if (!SkipAlways.Contains(extension, StringComparer.InvariantCultureIgnoreCase))
            {
                return Config.CompressModule.Extensions.OfType<CompressExtensionElement>().Where(e => e.Value.Equals(extension, StringComparison.InvariantCultureIgnoreCase) && e.Process).Any();
            }

            return false;
        }

        public CompressionType GetExtensionCompressinType(string extension)
        {
            if (!string.IsNullOrWhiteSpace(extension))
            {
                var ext = Config.CompressModule.Extensions.Cast<CompressExtensionElement>().Where(e => e.Value.Equals(extension, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                if (ext != null)
                {
                    return ext.CompressionType;
                }
            }

            return CompressionType.Deflate | CompressionType.GZip;

        }
    }


}
