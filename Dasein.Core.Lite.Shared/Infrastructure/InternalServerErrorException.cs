using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using System.Text;

namespace Dasein.Core.Lite.Shared
{
    public class InternalServerErrorException : HttpExceptionBase
    {
        public InternalServerErrorException()
        {
        }

        public InternalServerErrorException(string message) : base(message)
        {
        }

        public InternalServerErrorException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public InternalServerErrorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public override HttpStatusCode HttpStatusCode => HttpStatusCode.InternalServerError;

        public override string DefaultMessage => "Internal Server Error";
    }
}
