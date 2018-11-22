using AspNetCoreStarterPack.Error;
using AspNetCoreStarterPack.Infrastructure;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AspNetCoreStarterPack.Extensions
{
    public static class ResponseExtensions
    {
        public static string ContentTypeJson = "application/json";

        public static HttpResponse WithModel<T>(this HttpResponse response, T model)
        {
            var settings = AppCore.Instance.Get<JsonSerializerSettings>();
            response.ContentType = ContentTypeJson;
            response.WriteAsync(JsonConvert.SerializeObject(model, settings)).Wait();
            return response;
        }
        
        public static HttpResponse WithStatusCode(this HttpResponse response, HttpStatusCode httpStatusCode)
        {
            response.StatusCode = (int)httpStatusCode;
            return response;
        }

        public static HttpResponse WithError(this HttpResponse response, HttpServiceError error)
        {
            response.WithStatusCode(error.HttpStatusCode);
            response.WithModel(error.ServiceErrorModel);
            return response;
        }
    }
}
