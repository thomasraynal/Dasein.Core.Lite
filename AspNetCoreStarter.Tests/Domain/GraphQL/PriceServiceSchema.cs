using AspNetCoreStarterPack.GraphQL;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreStarter.Demo.Common.Domain.GraphQL
{
    public class PriceServiceSchema : GraphQLSchemaBase
    {
        public PriceServiceSchema()
        {
            Query = DependencyResolver.Resolve<PriceServiceQuery>();
        }
    }
}
