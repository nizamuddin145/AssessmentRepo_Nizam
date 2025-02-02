using System.Net.Http;
using System.Net;
using System.Text;
using System.Web.Http.Filters;
using System;
using System.Data.Entity.Core;

namespace WebApi.App_Start
{
    [AttributeUsage(AttributeTargets.Class)]
    public class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            var exception = context.Exception;

            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

            if (exception is ArgumentException ||
                exception is InvalidOperationException ||
                exception is ArgumentNullException || 
                exception is NullReferenceException)
            {
                statusCode = HttpStatusCode.BadRequest;
            }

            var response = new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(exception.Message, Encoding.UTF8, "application/json")
            };

            context.Response = response;
        }
    }
}