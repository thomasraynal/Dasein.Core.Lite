using AspNetCoreStarterPack.Error;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using System.Text;

namespace AspNetCoreStarterPack.Default
{
    [Serializable]
    public class MissingHubConfigException : Exception, IHasHttpServiceError
    {
        private readonly HttpServiceError _HttpServiceError;

        private HttpServiceError CreateModel()
        {
            return HttpServiceErrorDefinition.MakeError(HttpStatusCode.BadRequest, Message ?? "Unable to find hub.");
        }

        public MissingHubConfigException()
        {
            _HttpServiceError = CreateModel();
        }

        public MissingHubConfigException(string message) : base(message)
        {
            _HttpServiceError = CreateModel();
        }

        public MissingHubConfigException(string message, Exception innerException) : base(message, innerException)
        {
            _HttpServiceError = CreateModel();
        }

        protected MissingHubConfigException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            _HttpServiceError = CreateModel();
        }

        public HttpServiceError HttpServiceError => _HttpServiceError;
    }
}
