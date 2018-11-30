using Dasein.Core.Lite.Demo.Shared;
using Dasein.Core.Lite;
using GraphQL;

namespace Dasein.Core.Lite.Demo.Server
{
    public class TradesController : GraphQLControllerBase<TradeServiceSchema>
    {
        public TradesController(IDocumentExecuter documentExecuter) : base(documentExecuter)
        {
        }
    }
}
