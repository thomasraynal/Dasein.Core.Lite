using AspNetCoreStarterPack.Error;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreStarterPack.Default
{
    public class GeneralServiceErrorException : Exception, IHasHttpServiceError
    {
        public GeneralServiceErrorException()
            : base() { }

        public GeneralServiceErrorException(string message)
            : base(message) { }

        public GeneralServiceErrorException(string message, Exception innerException)
            : base(message, innerException) { }

        public HttpServiceError HttpServiceError => throw new NotImplementedException();
    }
}
