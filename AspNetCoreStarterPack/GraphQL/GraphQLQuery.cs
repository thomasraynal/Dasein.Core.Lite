using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreStarterPack.GraphQL
{
    public class GraphQLQuery
    {
        public string Query { get; set; }
        public JObject Variables { get; set; }
    }
}
