using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserControl.Api.CustomExceptionMiddleware
{
    public class ApiBadRequestResponse
    {

        public int StatusCode { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; }

        public ApiBadRequestResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        private static string GetDefaultMessageForStatusCode(int statusCode)
        {
            switch (statusCode)
            {
            
            case 453:
                return "Resource not found";
            case 500:
                return "An unhandled error occurred";
            default:
                return null;
        }
    }

}
}
