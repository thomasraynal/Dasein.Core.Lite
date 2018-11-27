using System;
using System.Runtime.Serialization;

namespace AspNetCoreStarterPack.GraphQL
{
    [Serializable]
    internal class InvalidGraphQLRequest : Exception
    {
        public InvalidGraphQLRequest()
        {
        }

        public InvalidGraphQLRequest(string message) : base(message)
        {
        }

        public InvalidGraphQLRequest(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidGraphQLRequest(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}