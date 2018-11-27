using AspNetCoreStarterPack.Extensions;
using AspNetCoreStarterPack.Infrastructure;
using GraphQL;
using GraphQL.Http;
using GraphQL.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreStarterPack.GraphQL
{
    public class GraphQLMiddleware<TGraphQLType, TEntity>: ICanLog where TGraphQLType : ObjectGraphType<TEntity>
    {
        private readonly RequestDelegate _next;

        public static string GraphQLEndpoint = "/graphql";

        public GraphQLMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (httpContext.Request.Path.Value.EndsWith(GraphQLEndpoint))
            {
                using (var stream = new StreamReader(httpContext.Request.Body))
                {
                    var query = await stream.ReadToEndAsync();
                    if (!String.IsNullOrWhiteSpace(query))
                    {
                        var objectGraph = AppCore.Instance.Get<TGraphQLType>();

                        var schema = new Schema
                        {
                            Query = objectGraph
                        };

                        var documentExecuter = AppCore.Instance.Get<IDocumentExecuter>();

                        var result = await documentExecuter.ExecuteAsync(options =>
                          {
                              options.Schema = schema;
                              options.Query = query;
                          });

                        await WriteResult(httpContext, result);
                    }
                }
            }
            else
            {
                await _next(httpContext);
            }
        }

        private async Task WriteResult(HttpContext httpContext, ExecutionResult result)
        {
            if (null != result.Errors && result.Errors.Any())
            {
                foreach(var error in result.Errors)
                {
                  this.LogError(error);
                }

                throw result.Errors.First();
            }

            var json = new DocumentWriter(indent: true).Write(result);

            httpContext.Response
                        .AsJson()
                        .WithStatusCode(System.Net.HttpStatusCode.OK);

            await httpContext.Response.WriteAsync(json);
        }
    }
}
