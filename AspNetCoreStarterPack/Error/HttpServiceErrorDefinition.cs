using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AspNetCoreStarterPack.Error
{
    public static class HttpServiceErrorDefinition
    {
        public static HttpServiceError NotFoundError = new HttpServiceError
        {
            HttpStatusCode = HttpStatusCode.NotFound,
            ServiceErrorModel = new ServiceErrorModel
            {
                Reason = nameof(HttpStatusCode.NotFound),
                Details = "The requested entity was not found."
            }
        };

        public static HttpServiceError GeneralError = new HttpServiceError
        {
            HttpStatusCode = HttpStatusCode.BadRequest,
            ServiceErrorModel = new ServiceErrorModel
            {
                Reason = nameof(HttpStatusCode.BadRequest),
                Details = "An error occured while processing the request."
            }
        };

        public static HttpServiceError InternalServerError = new HttpServiceError
        {
            HttpStatusCode = HttpStatusCode.InternalServerError,
            ServiceErrorModel = new ServiceErrorModel
            {
                Reason = nameof(HttpStatusCode.InternalServerError),
                Details = "There was an internal server error during processing the request."
            }
        };

        public static HttpServiceError MakeError(HttpStatusCode httpCode, String details, String reason = null)
        {
            return new HttpServiceError
            {
                HttpStatusCode = httpCode,
                ServiceErrorModel = new ServiceErrorModel
                {
                    Reason = reason ?? nameof(httpCode),
                    Details = details
                }
            };
        }
    }
}
