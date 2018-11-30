using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dasein.Core.Lite.Demo.Shared
{
    public class PriceServiceQuery : ObjectGraphType<IPrice>
    {
        public PriceServiceQuery(IPriceService repository)
        {
            Field<ListGraphType<PriceType>>("prices", arguments: new QueryArguments(new QueryArgument<StringGraphType>() { Name = "asset" }), resolve: (context) =>
            {
                var tradeId = context.GetArgument<string>("asset");
                return repository.GetPricesByAsset(tradeId);
            });

        }
    }
}
