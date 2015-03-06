using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace HtmlOptimization.Modules
{
    public class MinifyModule:IHttpModule
    {
        public void Dispose()
        {

        }

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
                !app.Context.Request.CurrentExecutionFilePathExtension.Equals(".aspx", StringComparison.InvariantCultureIgnoreCase) &&
                !app.Context.Request.CurrentExecutionFilePathExtension.Equals(".axd", StringComparison.InvariantCultureIgnoreCase))
            {
                context.Response.Filter = new HtmlOptimization.Modules.HtmlMinify.HtmlMinifyFilter(context);

            }
        }


    }
}
