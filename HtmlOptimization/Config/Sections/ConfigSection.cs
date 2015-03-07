using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlOptimization.Config.Elements;

namespace HtmlOptimization.Config.Sections
{
    public class ConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("CompressionModule", IsRequired = false)]
        public CompressModuleElement CompressionModule
        {
            get
            {
                return (CompressModuleElement)base["CompressionModule"];
            }
        }

        [ConfigurationProperty("MinifyModule", IsRequired = false)]
        public MinifyModuleElement Cache
        {
            get
            {
                return (MinifyModuleElement)base["MinifyModule"];
            }
        }
    }
}
