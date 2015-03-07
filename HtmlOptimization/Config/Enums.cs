using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlOptimization.Config
{
    [Flags]
    public enum CompressionType
    {
        None = 0,
        Deflate = 1,
        GZip = 2
    }
}
