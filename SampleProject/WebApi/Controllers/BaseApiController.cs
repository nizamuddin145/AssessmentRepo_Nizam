using Microsoft.Ajax.Utilities;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace WebApi.Controllers
{
    public abstract class BaseApiController : ApiController
    {
        /// <summary>
        /// Creates a Http response with status Created and the object as a response content. 
        /// </summary>
        /// <param name="value">Request body as value.</param>
        /// <returns>HttpResponseMessage with Created status and the provided object as content.<returns>
        public HttpResponseMessage DoesCreated(object value)
        {
            return new HttpResponseMessage(HttpStatusCode.Created)
            {
                Content = new ObjectContent(value.GetType(), value, new JsonMediaTypeFormatter())
            };
        }

        /// <summary>
        /// Creates a Http response with status NotFound. 
        /// </summary>
        /// <returns>HttpResponseMessage with NotFound status.<returns>
        public HttpResponseMessage DoesNotExist()
        {
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Http response with status Found and the object as a response content. 
        /// </summary>
        /// <param name="value">Request body as value.</param>
        /// <returns>HttpResponseMessage with NotFound status and the provided object as content.<returns>
        public HttpResponseMessage Found(object value)
        {
            return new HttpResponseMessage(HttpStatusCode.Found)
            {
                Content = new ObjectContent(value.GetType(), value, new JsonMediaTypeFormatter())
            };
        }

        /// <summary>
        /// Http response with status OK. 
        /// </summary>
        /// <returns>HttpResponseMessage with OK status.<returns>
        public HttpResponseMessage OK()
        {
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        /// <summary>
        /// Http response with status OK and the object as a response content. 
        /// </summary>
        /// <param name="value">Request body as value.</param>
        /// <returns>HttpResponseMessage with OK status and the provided object as content.<returns>
        public HttpResponseMessage OK(object value)
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent(value.GetType(), value, new JsonMediaTypeFormatter())
            };
        }

        /// <summary>
        /// Http response with status InternalServerError and the error message as a response content. 
        /// </summary>
        /// <param name="value">Error message.</param>
        /// <returns>HttpResponseMessage with InternalServerError status and the error message as content.<returns>
        public HttpResponseMessage ServerError(string errorMessage)
        {
            return new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(errorMessage)
            };
        }

        /// <summary>
        /// Http response with status BadRequest and the error message as a response content. 
        /// </summary>
        /// <param name="value">Error message.</param>
        /// <returns>HttpResponseMessage with BadRequest status and the error message as content.<returns>
        public HttpResponseMessage BadRequestFound(object value)
        {
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new ObjectContent(value.GetType(), value, new JsonMediaTypeFormatter())
            };
        }
    }
}