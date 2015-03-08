using HtmlOptimization.Config.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace HtmlOptimization.Modules
{
    public class MinifyModule :ModuleBase, IHttpModule
    {

        public void Init(HttpApplication context)
        {
            context.PostRequestHandlerExecute += context_PostRequestHandlerExecute;
        }

        void context_PostRequestHandlerExecute(object sender, EventArgs e)
        {
            HttpApplication app = sender as HttpApplication;
            HttpContext context = app.Context;
            string contentType = app.Context.Response.ContentType;



            if (context.Request.HttpMethod == "GET" &&
                contentType.Equals("text/html") &&
                context.Response.StatusCode == 200 &&
                context.CurrentHandler != null &&
                string.IsNullOrWhiteSpace(app.Context.Request.CurrentExecutionFilePathExtension) ? true : ProcessExtension(app.Context.Request.CurrentExecutionFilePathExtension))
            {
                context.Response.Filter = new HtmlOptimization.Modules.HtmlMinify.HtmlMinifyFilter(context);

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
                return Config.MinifyModule.Extensions.OfType<ExtensionElement>().Where(e => e.Value.Equals(extension, StringComparison.InvariantCultureIgnoreCase) && e.Process).Any();
            }
            return false;
            
        }
    }
}
