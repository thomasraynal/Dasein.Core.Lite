using Dasein.Core.Lite.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Dasein.Core.Lite.Shared
{
    public class MissingServiceException : HttpExceptionBase
    {
        public MissingServiceException()
        {
        }

        public MissingServiceException(string message) : base(message)
        {
        }

        public MissingServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public MissingServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public override HttpStatusCode HttpStatusCode => HttpStatusCode.InternalServerError;

        public override string DefaultMessage => "Missing service";
    }
}
