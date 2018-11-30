using Dasein.Core.Lite.Shared;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dasein.Core.Lite.Demo.Shared
{
    public class TradeServiceSchema : GraphQLSchemaBase
    {
        public TradeServiceSchema()
        {
            Query = DependencyResolver.Resolve<TradeServiceQuery>();
        }
    }
}
