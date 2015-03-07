using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlOptimization.Config.Sections
{
    public abstract class ConfigSectionBase : ConfigurationSection
    {
        [ConfigurationProperty("xmlns", IsRequired = false)]
        public String Xmlns
        {
            get
            {
                return this["xmlns"] != null ? this["xmlns"].ToString() : string.Empty;
            }
        }

    }
}
