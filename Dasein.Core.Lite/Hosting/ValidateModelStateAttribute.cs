using Dasein.Core.Lite.Shared.Infrastructure;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dasein.Core.Lite.Hosting
{
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values.SelectMany(m => m.Errors)
                                 .Select(e => e.ErrorMessage)
                                 .ToList()
                                 .Aggregate((err1, err2) => $"{err1}{Environment.NewLine}{err2}");

                throw new RequestException(errors);

            }
        }
    }
}
