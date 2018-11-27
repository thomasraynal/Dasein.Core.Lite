using AspNetCoreStarter.Demo.Common.Domain.GraphQL;
using AspNetCoreStarterPack.GraphQL;
using GraphQL;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreStarter.Demo.Common.Module
{
    public class PricesController : GraphQLControllerBase<PriceServiceSchema>
    {
        public PricesController(IDocumentExecuter documentExecuter) : base(documentExecuter)
        {
        }
    }
}
