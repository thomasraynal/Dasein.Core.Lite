using Dasein.Core.Lite.Shared;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using System.Text;

namespace Dasein.Core.Lite.Shared
{
    public class UnauthorizedUserException : HttpExceptionBase
    {
        public UnauthorizedUserException()
        {
        }

        public UnauthorizedUserException(string message) : base(message)
        {
        }

        public UnauthorizedUserException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public UnauthorizedUserException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public override HttpStatusCode HttpStatusCode => HttpStatusCode.Unauthorized;

        public override string DefaultMessage => "User must be authenticated to access this service";
    }
}
