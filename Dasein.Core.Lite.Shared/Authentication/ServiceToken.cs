using System;
using System.Collections.Generic;
using System.Text;

namespace Dasein.Core.Lite.Shared
{
    public abstract class ServiceToken : IServiceToken
    {
        public String Digest { get; set; }
    }
}
