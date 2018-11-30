using Dasein.Core.Lite.Shared;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dasein.Core.Lite.Demo.Shared
{
    public class GraphQLQueryValidator : AbstractValidator<GraphQLQuery>
    {
        public GraphQLQueryValidator()
        {
            RuleFor(request => request.Query).NotEmpty().WithMessage("Query should be set");
        }
    }
}
