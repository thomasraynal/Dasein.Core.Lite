using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using System.Text;

namespace Dasein.Core.Lite.Shared
{
    [Serializable]
    public class MissingHubConfigException : HttpExceptionBase
    {
        public MissingHubConfigException()
        {
        }

        public MissingHubConfigException(string message) : base(message)
        {
        }

        public MissingHubConfigException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public MissingHubConfigException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public override HttpStatusCode HttpStatusCode => HttpStatusCode.InternalServerError;

        public override string DefaultMessage => "Unable to find hub.";
    }
}
