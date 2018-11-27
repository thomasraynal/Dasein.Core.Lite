using AspNetCoreStarterPack.GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreStarter.Demo.Common.Domain.GraphQL
{
    public class TradeServiceSchema : GraphQLSchemaBase
    {
        public TradeServiceSchema()
        {
            Query = DependencyResolver.Resolve<TradeServiceQuery>();
        }
    }
}
