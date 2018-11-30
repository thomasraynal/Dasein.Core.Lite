using Dasein.Core.Lite.Shared;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dasein.Core.Lite.Demo.Shared
{
    public class PriceType : ObjectGraphType<IPrice>
    {
        public PriceType()
        {
            Field(price => price.Id, type: typeof(GraphQLGenericGraphType));
            Field(price => price.Asset);
            Field(price => price.Value);
            Field(price => price.Date, type: typeof(GraphQLGenericGraphType));
        }
    }
}
