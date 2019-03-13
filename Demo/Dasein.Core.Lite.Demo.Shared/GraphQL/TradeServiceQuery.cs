//using GraphQL.Types;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;

//namespace Dasein.Core.Lite.Demo.Shared
//{
//    public class TradeServiceQuery : ObjectGraphType<ITrade>
//    {

//        public TradeServiceQuery(ITradeService repository, IPriceService priceService)
//        {
//            Field<TradeType>("trade", arguments: new QueryArguments(new QueryArgument<StringGraphType>() { Name = "id" }), resolve: (ResolveFieldContext<ITrade> context) =>
//            {
//                var id = context.GetArgument<Guid>("id");
//                return repository.GetTradeById(id);
//            });

//            Field<ListGraphType<TradeType>>("trades", resolve: context =>
//            {
//                return repository.GetAllTrades();
//            });

//            Field<ListGraphType<PriceType>>("prices", arguments: new QueryArguments(new QueryArgument<StringGraphType>() { Name = "tradeId" }), resolve: (context) =>
//            {
//                var tradeId = context.GetArgument<Guid>("tradeId");
//                return priceService.GetPricesByTrade(tradeId);
//            });

//        }
//    }
//}
