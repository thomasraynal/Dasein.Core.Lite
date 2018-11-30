using Dasein.Core.Lite.Shared;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dasein.Core.Lite
{
    public class ErrorHandlerMiddleware : ICanLog
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                this.LogError(ex);
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var httpServiceError = HttpServiceErrorUtilities.ExtractFromException(exception, HttpServiceErrorDefinition.GeneralError);
            context.Response.WithError(httpServiceError);
            return Task.CompletedTask;
        }
    }
}
