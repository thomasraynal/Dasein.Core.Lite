using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Serialization = Serialize.Linq.Serializers;

namespace Dasein.Core.Lite.Shared
{
    public class HubRequestFilter : IHubRequestFilter
    {
        public HubRequestFilter(Expression filterExpression)
        {
            FilterExpression = filterExpression;

            var expressionSerializer = new Serialization.ExpressionSerializer(new Serialization.JsonSerializer());
            var json = expressionSerializer.SerializeText(FilterExpression);
            GroupId = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
        }

        public static HubRequestFilter FromBase64(String filter)
        {
            var expressionSerializer = new Serialization.ExpressionSerializer(new Serialization.JsonSerializer());
            var base64Request = Encoding.UTF8.GetString(Convert.FromBase64String(filter));
            var expression = expressionSerializer.DeserializeText(base64Request);
            return new HubRequestFilter(expression);
        }

        public Expression FilterExpression { get; private set; }
        public String GroupId { get; private set; }

        public override bool Equals(object obj)
        {
            var cast = (IHubRequestFilter)obj;

            return cast != null && GroupId == cast.GroupId;
        }

        public override int GetHashCode()
        {
            return GroupId.GetHashCode();
        }
    }
}
