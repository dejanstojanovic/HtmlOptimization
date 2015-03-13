using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlOptimization.Config.Elements
{
    public class HtmlImageThumbnailElement : ConfigurationElement
    {
        [ConfigurationProperty("cacheFolder", DefaultValue = "/ThumbnailCache", IsRequired = false)]
        public string CacheFolder
        {
            get
            {
                return this["cacheFolder"] as string;
            }
        }

        [ConfigurationProperty("createCacheFolder", DefaultValue = true, IsRequired = false)]
        public Boolean CreateCacheFolder
        {
            get
            {
                return (Boolean)this["createCacheFolder"];
            }
        }
    }
}
