using AspNetCoreStarter.Tests.Domain;
using AspNetCoreStarterPack.GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreStarter.Demo.Common.Domain
{
    public class TradeType : ObjectGraphType<ITrade>
    {
        public TradeType()
        {
            Field(trade => trade.Id, type: typeof(GraphQLGenericGraphType));
            Field(trade => trade.Asset);
            Field(trade => trade.Counterparty);
            Field(trade => trade.Date, type: typeof(GraphQLGenericGraphType));
            Field(trade => trade.PriceOnTransaction);
            Field(trade => trade.Status, type: typeof(GraphQLGenericGraphType));
            Field(trade => trade.Way, type: typeof(GraphQLGenericGraphType));
            Field(trade => trade.Volume);
        }
    }
}
