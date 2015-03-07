using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Web;

namespace HtmlOptimization.Modules
{
    public class CompressModule:IHttpModule
    {
        public void Dispose()
        {

        }

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
                //!app.Context.Request.CurrentExecutionFilePath.StartsWith("/umbraco/",StringComparison.InvariantCultureIgnoreCase) &&
                //!app.Context.Request.CurrentExecutionFilePath.StartsWith("/__browserLink/", StringComparison.InvariantCultureIgnoreCase) &&
                !app.Context.Request.CurrentExecutionFilePathExtension.Equals(".aspx",StringComparison.InvariantCultureIgnoreCase) &&
                !app.Context.Request.CurrentExecutionFilePathExtension.Equals(".axd",StringComparison.InvariantCultureIgnoreCase)
                )
            {
                if (acceptEncoding == null || acceptEncoding.Length == 0)
                    return;

                acceptEncoding = acceptEncoding.ToLower();

                if (acceptEncoding.Contains("deflate") || acceptEncoding == "*")
                {
                    // deflate
                    app.Response.Filter = new DeflateStream(prevUncompressedStream,
                        CompressionMode.Compress);
                    app.Response.AppendHeader("Content-Encoding", "deflate");
                }
                else if (acceptEncoding.Contains("gzip"))
                {
                    // gzip
                    app.Response.Filter = new GZipStream(prevUncompressedStream,
                        CompressionMode.Compress);
                    app.Response.AppendHeader("Content-Encoding", "gzip");
                }
            }
        }

       

    }
}
