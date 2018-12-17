using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using System.Text;

namespace Dasein.Core.Lite.Shared.Infrastructure
{
    public class RequestException : HttpExceptionBase
    {
        public RequestException()
        {
        }

        public RequestException(string message) : base(message)
        {
        }

        public RequestException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public RequestException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public override HttpStatusCode HttpStatusCode => HttpStatusCode.BadRequest;

        public override string DefaultMessage => "Bad Request";
    }
}
