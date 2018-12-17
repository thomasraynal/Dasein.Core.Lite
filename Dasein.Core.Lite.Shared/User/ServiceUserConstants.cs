using System;
using System.Collections.Generic;
using System.Text;

namespace Dasein.Core.Lite.Shared
{
    public static class ServiceUserConstants
    {
        public const string BearerAuthenticationScheme = "Bearer";
        public const string Expiration = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/service/expiration";
        public const string Digest = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/service/digest";
    }
}
