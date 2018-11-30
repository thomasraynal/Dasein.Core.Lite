using Dasein.Core.Lite.Shared;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using System.Text;

namespace Dasein.Core.Lite.Shared
{
    public abstract class HttpExceptionBase : Exception, IHasHttpServiceError
    {
        private HttpServiceError _HttpServiceError;

        protected void CreateModel()
        {
            _HttpServiceError = HttpServiceErrorDefinition.MakeError(HttpStatusCode, Message ?? DefaultMessage);
        }

        public abstract HttpStatusCode HttpStatusCode { get; }
        public abstract String DefaultMessage { get; }

        public HttpExceptionBase()
        {
            CreateModel();
        }

        public HttpExceptionBase(string message) : base(message)
        {
            CreateModel();
        }

        public HttpExceptionBase(string message, Exception innerException) : base(message, innerException)
        {
            CreateModel();
        }

        protected HttpExceptionBase(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            CreateModel();
        }


        public HttpServiceError HttpServiceError => _HttpServiceError;
    }
}
