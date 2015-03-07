using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlOptimization.Config.Elements
{
    public class CompressExtensionElement:ExtensionElement
    {
        [ConfigurationProperty("compressionType", DefaultValue = CompressionType.Deflate, IsRequired = false)]
        public CompressionType CompressionType
        {
            get
            {
                return (CompressionType)this["compressionType"];
            }
        }
    }
}
