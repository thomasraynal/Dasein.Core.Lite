using AspNetCoreStarterPack.Error;
using GraphQL;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;

namespace AspNetCoreStarterPack.GraphQL
{
    [Serializable]
    internal class InvalidGraphQLRequest : Exception, IHasHttpServiceError
    {
        private HttpServiceError _HttpServiceError;

        private HttpServiceError CreateModel()
        {
            return HttpServiceErrorDefinition.MakeError(HttpStatusCode.BadRequest, Message ?? "Unable to interpret the query");
        }

        public InvalidGraphQLRequest()
        {
            _HttpServiceError = CreateModel();
        }

        public InvalidGraphQLRequest(string message) : base(message)
        {
            _HttpServiceError = CreateModel();
        }

        public InvalidGraphQLRequest(ExecutionErrors errors):base(JsonConvert.SerializeObject(errors.Select((error)=> error.Message)))
        {
            _HttpServiceError = CreateModel();
        }

        public InvalidGraphQLRequest(string message, Exception innerException) : base(message, innerException)
        {
            _HttpServiceError = CreateModel();
        }

        protected InvalidGraphQLRequest(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            _HttpServiceError = CreateModel();
        }

        public HttpServiceError HttpServiceError => _HttpServiceError;
    }
}