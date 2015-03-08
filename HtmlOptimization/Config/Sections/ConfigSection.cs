using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlOptimization.Config.Elements;

namespace HtmlOptimization.Config.Sections
{
    public class ConfigSection : ConfigSectionBase
    {
        [ConfigurationProperty("compressModule", IsRequired = false)]
        public CompressModuleElement CompressModule
        {
            get
            {
                return (CompressModuleElement)base["compressModule"];
            }
        }

        [ConfigurationProperty("minifyModule", IsRequired = false)]
        public MinifyModuleElement MinifyModule
        {
            get
            {
                return (MinifyModuleElement)base["minifyModule"];
            }
        }
    }
}
