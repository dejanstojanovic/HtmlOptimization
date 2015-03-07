using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlOptimization.Config.Elements
{
    public class CompressExtensionCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new CompressExtensionElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CompressExtensionElement)element).Value;
        }

        public void Add(CompressExtensionElement extension)
        {
            BaseAdd(extension);
        }

        public void Clear()
        {
            BaseClear();
        }
    }
}
