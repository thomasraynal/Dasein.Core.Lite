using AspNetCoreStarterPack.Error;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using System.Text;

namespace AspNetCoreStarter.Tests.Infrastructure
{
    public class UnauthorizedUserException : Exception, IHasHttpServiceError
    {
        private readonly HttpServiceError _HttpServiceError;

        private HttpServiceError CreateModel(String message = null)
        {
            return new HttpServiceError()
            {
                HttpStatusCode = HttpStatusCode.Unauthorized,
                ServiceErrorModel = new ServiceErrorModel()
                {
                    Details = message ?? "User must be authenticated to access this service",
                    Reason = nameof(HttpStatusCode.Unauthorized)
                }
            };
        }

        public UnauthorizedUserException()
        {
            _HttpServiceError = CreateModel();
        }

        public UnauthorizedUserException(string message) : base(message)
        {
            _HttpServiceError = CreateModel(message);
        }

        public UnauthorizedUserException(string message, Exception innerException) : base(message, innerException)
        {
            _HttpServiceError = CreateModel(message);
        }

        protected UnauthorizedUserException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            _HttpServiceError = CreateModel();
        }

        public HttpServiceError HttpServiceError => _HttpServiceError;
    }
}
