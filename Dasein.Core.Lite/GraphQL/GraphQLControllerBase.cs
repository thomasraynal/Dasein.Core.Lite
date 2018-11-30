using Dasein.Core.Lite.Shared;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dasein.Core.Lite
{
    [Route("api/v{version:apiVersion}/graphql/[controller]")]
    public abstract class GraphQLControllerBase<TSchema> : Controller, ICanLog
        where TSchema : ISchema , new()
    {
        private ISchema _schema;
        private IDocumentExecuter _documentExecuter;

        public GraphQLControllerBase(IDocumentExecuter documentExecuter)
        {
            _schema = new TSchema();
            _documentExecuter = documentExecuter;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GraphQLQuery query)
        {
            if (null == query) throw new InvalidGraphQLRequestException("GraphQLQuery cannot be resolved");

            var inputs = query.Variables.ToInputs();

            var executionOptions = new ExecutionOptions
            {
                Schema = _schema,
                Query = query.Query,
                Inputs = inputs
            };

            var result = await _documentExecuter.ExecuteAsync(executionOptions);

            if (result.Errors?.Count > 0)
            {
                throw new InvalidGraphQLRequestException(result.Errors);
            }

            return Ok(result);
        }
    }
}
