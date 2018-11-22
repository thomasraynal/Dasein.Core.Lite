using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreStarter.Authentication
{
    public abstract class ServiceToken : IServiceToken
    {
        public String Digest { get; set; }
    }
}
