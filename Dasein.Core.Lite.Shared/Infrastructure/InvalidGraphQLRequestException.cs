using Dasein.Core.Lite.Shared;
using GraphQL;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;

namespace Dasein.Core.Lite.Shared
{
    public class InvalidGraphQLRequestException : HttpExceptionBase
    {
        public InvalidGraphQLRequestException()
        {
        }

        public InvalidGraphQLRequestException(ExecutionErrors errors) : base(JsonConvert.SerializeObject(errors.Select((error) => error.Message)))
        {
            CreateModel();
        }

        public InvalidGraphQLRequestException(string message) : base(message)
        {
        }

        public InvalidGraphQLRequestException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public InvalidGraphQLRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public override HttpStatusCode HttpStatusCode => HttpStatusCode.BadRequest;

        public override string DefaultMessage => "Unable to interpret the query";
    }
}