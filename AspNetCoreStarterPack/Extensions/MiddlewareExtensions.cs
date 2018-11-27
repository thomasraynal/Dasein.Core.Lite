using AspNetCoreStarterPack.Extensions;
using AspNetCoreStarterPack.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreStarterPack.Error
{
    public static class MiddlewareExtensions
    {
        public static void UseServiceExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
