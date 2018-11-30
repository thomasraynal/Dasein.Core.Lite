using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using System.Text;

namespace Dasein.Core.Lite.Shared
{
    public class ForbiddenException : HttpExceptionBase
    {
        public ForbiddenException()
        {
        }

        public ForbiddenException(string message) : base(message)
        {
        }

        public ForbiddenException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ForbiddenException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public override HttpStatusCode HttpStatusCode => HttpStatusCode.Forbidden;

        public override string DefaultMessage => "Forbidden";
    }
}
