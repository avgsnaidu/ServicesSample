using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace VirtusMobileAPI.Models
{
    public class UnhandledExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            HttpStatusCode status = HttpStatusCode.BadRequest;

            var exType = context.Exception.GetType();

            if (exType == typeof(UnauthorizedAccessException))
                status = HttpStatusCode.Unauthorized;
            else if (exType == typeof(ArgumentException))
                status = HttpStatusCode.NotFound;

            // create a new response and attach our ApiError object
            // which now gets returned on ANY exception result
            //context.Response = context.Request.CreateResponse<ApiMessageError>(status, apiError);
            context.Response = new System.Net.Http.HttpResponseMessage()
            {
                StatusCode=status,
                Content = new StringContent(context.Exception.Message)
            };
        }
    }
}