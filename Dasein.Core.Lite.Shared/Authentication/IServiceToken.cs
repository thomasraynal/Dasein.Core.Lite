using System;
using System.Collections.Generic;
using System.Text;

namespace Dasein.Core.Lite.Shared
{
    public interface IServiceToken
    {
        string Digest { get; set; }
    }
}
