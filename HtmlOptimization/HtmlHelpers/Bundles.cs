using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Optimization;

namespace HtmlOptimization.HtmlHelpers
{
    public static class Bundles
    {

        #region Java Script
        public enum ScriptLoading
        {
            /// <summary>
            /// Creates normal blocking script script tag
            /// </summary>
            [Description("Creates normal blocking script script tag")]
            None,
            /// <summary>
            /// Adds defer attribute for script tag
            /// </summary>
            [Description("Adds defer attribute for script tag")]
            Defer,
            /// <summary>
            /// Adds async attribute for script tag
            /// </summary>
            [Description("Adds async attribute for script tag")]
            Async
        }

        public static IHtmlString Scripts(string bundleName, bool optimized, ScriptLoading loading = ScriptLoading.None, params string[] paths)
        {
            var newBundleName = string.Format("{0}/{1}", "~/bundles", bundleName);

            if (HttpContext.Current.Cache[newBundleName] == null)
            {
                var bundle = new ScriptBundle(newBundleName);
                var files = new List<string>();
                foreach (var path in paths)
                {
                    var mappedFile = path;


                    if (BundleTable.VirtualPathProvider.FileExists(mappedFile))
                    {
                        files.Add(HttpContext.Current.Server.MapPath(mappedFile));
                        bundle.Include(mappedFile);
                    }
                    else
                    {
                        throw new Exception(string.Format("Script not found : {0}", mappedFile));
                    }
                }

                BundleTable.Bundles.Add(bundle);
                BundleTable.EnableOptimizations = optimized;

                var scriptTagFormat = string.Concat("<script type=\"text/javascript\" ", loading != ScriptLoading.None ? loading.ToString().ToLower() : string.Empty, " src=\"{0}\"></script>");
                var newBundle = System.Web.Optimization.Scripts.RenderFormat(
                    scriptTagFormat,
                    newBundleName
                    );

                HttpContext.Current.Cache.Insert(newBundleName, newBundle, new CacheDependency(files.ToArray()));

                return newBundle;
            }
            else
            {
                return new HtmlString(HttpContext.Current.Cache[newBundleName].ToString());
            }
        }

        public static IHtmlString Scripts(string bundleName, bool optimized, params string[] paths)
        {
            return Scripts(bundleName, optimized, ScriptLoading.None, paths);
        }

        #endregion

        #region Stylesheet
        public enum MediaType
        {
            /// <summary>
            /// Used for all media type devices
            /// </summary>
            [Description("Used for all media type devices")]
            All = 0,
            /// <summary>
            /// Used for speech and sound synthesizers
            /// </summary>
            [Description("Used for speech and sound synthesizers")]
            Aural = 1,
            /// <summary>
            /// Used for braille tactile feedback devices
            /// </summary>
            [Description("Used for braille tactile feedback devices")]
            Braille = 2,
            /// <summary>
            /// Used for paged braille printers
            /// </summary>
            [Description("Used for paged braille printers")]
            Embossed = 3,
            /// <summary>
            /// Used for small or handheld devices
            /// </summary>
            [Description("Used for small or handheld devices")]
            Handheld = 4,
            /// <summary>
            /// Used for printers
            /// </summary>
            [Description("Used for printers")]
            Print = 5,
            /// <summary>
            /// Used for projected presentations, like slides
            /// </summary>
            [Description("Used for projected presentations, like slides")]
            Projection = 6,
            /// <summary>
            /// Used for computer screens
            /// </summary>
            [Description("Used for computer screens")]
            screen = 7,
            /// <summary>
            /// Used for media using a fixed-pitch character grid, like teletypes and terminals
            /// </summary>
            [Description("Used for media using a fixed-pitch character grid, like teletypes and terminals")]
            Tty = 8,
            /// <summary>
            /// Used for television-type devices
            /// </summary>
            [Description("Used for television-type devices")]
            Tv = 9
        }

        public static IHtmlString Styles(string bundleName, bool optimized, MediaType media = MediaType.All, params string[] paths)
        {
            var newBundleName = string.Format("{0}/{1}", "~/bundles", bundleName);

            if (HttpContext.Current.Cache[newBundleName] == null)
            {
                var bundle = new StyleBundle(newBundleName);
                var files = new List<string>();
                foreach (var path in paths)
                {
                    var mappedFile = path;

                    if (BundleTable.VirtualPathProvider.FileExists(mappedFile))
                    {
                        files.Add(HttpContext.Current.Server.MapPath(mappedFile));
                        bundle.Include(mappedFile);
                    }
                    else
                    {
                        throw new Exception(string.Format("Styles not found : {0}", mappedFile));
                    }
                }

                BundleTable.Bundles.Add(bundle);
                BundleTable.EnableOptimizations = optimized;

                var styleTagFormat = string.Concat("<link rel=\"stylesheet\" type=\"text/css\" media=\"", media.ToString().ToLower(), "\" href=\"{0}\" />");
                var newBundle = System.Web.Optimization.Styles.RenderFormat(styleTagFormat, newBundleName);

                HttpContext.Current.Cache.Insert(newBundleName, newBundle, new CacheDependency(files.ToArray()));

                return newBundle;
            }
            else
            {
                return new HtmlString(HttpContext.Current.Cache[newBundleName].ToString());
            }
        }

        #endregion
    }
}
