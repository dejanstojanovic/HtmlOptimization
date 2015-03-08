using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlOptimization.Config.Elements
{
    public class ExtensionElement : ConfigurationElement
    {
        [ConfigurationProperty("value", IsRequired = true)]
        public String Value
        {
            get
            {
                return (String)this["value"];
            }
        }

        [ConfigurationProperty("process", DefaultValue =false, IsRequired = true)]
        public bool Process
        {
            get
            {
                return (bool)this["process"];
            }
        }
    }
}
