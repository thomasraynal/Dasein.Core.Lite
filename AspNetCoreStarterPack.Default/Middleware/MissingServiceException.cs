using AspNetCoreStarterPack.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreStarterPack.Default
{
    public class MissingServiceException : Exception, IHasHttpServiceError
    {
        private HttpServiceError _HttpServiceError;

        private HttpServiceError CreateModel()
        {
            return HttpServiceErrorDefinition.MakeError(HttpStatusCode.InternalServerError, Message ?? "Missing service");
        }

        public MissingServiceException()
        {
            CreateModel();
        }

        public MissingServiceException(string message) : base(message)
        {
            CreateModel();
        }

        public MissingServiceException(string message, Exception innerException) : base(message, innerException)
        {
            CreateModel();
        }

        protected MissingServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            CreateModel();
        }


        public HttpServiceError HttpServiceError => _HttpServiceError;
    }
}
