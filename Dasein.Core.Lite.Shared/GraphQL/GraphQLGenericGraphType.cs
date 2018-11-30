using GraphQL;
using GraphQL.Language.AST;
using GraphQL.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dasein.Core.Lite.Shared
{
    public class GraphQLGenericGraphType : ScalarGraphType
    {
       public JsonSerializerSettings Settings
        {
            get
            {
                return AppCore.Instance.Get<JsonSerializerSettings>();
            }
        }

        public override object ParseLiteral(IValue value)
        {
            if (value is StringValue str)
            {
                return ParseValue(str.Value);
            }
            return null;
        }

        public override object ParseValue(object value)
        {
            var obj = value?.ToString();
            if (null == obj) return null;

            return JsonConvert.DeserializeObject(obj, Settings);
        }

        public override object Serialize(object value)
        {
            if (null == value) return null;
            return JsonConvert.SerializeObject(value, Settings).Trim(' ', '"');
        }
    }
}
