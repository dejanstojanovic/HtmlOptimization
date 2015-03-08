using HtmlOptimization.Config.Elements;
using HtmlOptimization.Config.Sections;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlOptimization.Modules
{
    public abstract class ModuleBase
    {
        protected static string[] skipAlways = new string[] { ".css", ".js" };
        protected static Config.Sections.ConfigSection config = ConfigurationManager.GetSection("htmlOptimization") as ConfigSection;

        public static string[] SkipAlways
        {
            get
            {
                return skipAlways;
            }
        }

        public static Config.Sections.ConfigSection Config
        {
            get
            {
                return config;
            }
        }

        public abstract bool ProcessExtension(string extension);

    }
}
