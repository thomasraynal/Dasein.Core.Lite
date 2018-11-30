using Dasein.Core.Lite.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dasein.Core.Lite
{
    public static class HttpServiceErrorUtilities
    {
        public static HttpServiceError ExtractFromException(Exception exception, HttpServiceError defaultValue)
        {
            var result = defaultValue;

            if (exception != null)
            {
                var exceptionWithServiceError = exception as IHasHttpServiceError;

                if (exceptionWithServiceError != null)
                {
                    result = exceptionWithServiceError.HttpServiceError;
                }
            }

            return result;
        }
    }
}
