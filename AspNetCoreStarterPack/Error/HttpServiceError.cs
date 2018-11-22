using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AspNetCoreStarterPack.Error
{
    public class HttpServiceError
    {
        public ServiceErrorModel ServiceErrorModel { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
    }
}
