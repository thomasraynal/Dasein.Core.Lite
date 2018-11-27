using AspNetCoreStarter.Tests.Domain;
using AspNetCoreStarterPack.GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreStarter.Demo.Common.Domain
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
