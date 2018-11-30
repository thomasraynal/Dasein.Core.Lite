using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dasein.Core.Lite.Shared
{
    public class GraphQLQuery
    {
        public string Query { get; set; }
        public JObject Variables { get; set; }
    }
}
