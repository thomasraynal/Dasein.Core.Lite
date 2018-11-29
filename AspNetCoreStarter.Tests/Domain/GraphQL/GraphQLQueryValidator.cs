using AspNetCoreStarterPack.GraphQL;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreStarter.Demo.Common.Domain.GraphQL
{
    public class GraphQLQueryValidator : AbstractValidator<GraphQLQuery>
    {
        public GraphQLQueryValidator()
        {
            RuleFor(request => request.Query).NotEmpty().WithMessage("Query should be set");
        }
    }
}
