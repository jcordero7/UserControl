
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using UserControl.Core.Exeptions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text;

namespace UserControl.Infrastructure.Filters
{
    using System.Net.Http;
    using System.Web;
    using System.Web.Http;

    //public class GlobalExceptionFilter : ExceptionFilterAttribute, IExceptionFilter

    //public class GlobalExceptionFilter : ExceptionFilterAttribute, IExceptionFilter
    //{
    //    public override void OnException(ExceptionContext context)
    //    {
    //        if (context.Exception.GetType() == typeof(BusinessExceptions))
    //        {
    //            var exception = (BusinessExceptions)context.Exception;

    //            /////respaldo
    //            ////var validation = new
    //            ////{
    //            ////    Status = 400,
    //            ////    Title = "Bad Request",
    //            ////    Message = exception.Message
    //            ////};
    //            ///

    //            //System.Runtime.ExceptionServices.ExceptionDispatchInfo fdfd = new System.Runtime.ExceptionServices.ExceptionDispatchInfo()

    //            // fdfd.
    //            context.Exception.Source = exception.Message;

    //            context.Exception = new Exception(exception.Message);
    //            context.Result = new ContentResult
    //            {
    //                Content = $"Error: {exception.Message}",
    //                ContentType = "application/json",
    //                // change to whatever status code you want to send out
    //                StatusCode = (int?)HttpStatusCode.BadRequest
    //            };

    //            //context.Result = new  BadRequestObjectResult(context);
    //            //context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

    //            //Detail = exception.Message

    //            //var json = new
    //            //{
    //            //    errors = new[] { validation }
    //            //};

    //            // context.Result = new BadRequestObjectResult(json);
    //            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

    //        }


    //        //System.Net.Http.HttpResponseMessage hola = new System.Net.Http.HttpResponseMessage();

    //        //System.Net.Http.HttpRequestMessage fdfd = new System.Net.Http.HttpRequestMessage();

    //        //System.Net.Http.HttpContent sdfsd;





    //        //  var message = new StringContent($"{this.EntityType ?? "Entity"} with id {this.EntityId ?? "(no id)"} already exist in the database");

    //        // throw new HttpResponseMessage(()

    //        //var message = new StringContent("Este es un error de prueba");

    //        //var sipi = new System.Net.Http.HttpResponseMessage(HttpStatusCode.BadRequest) { Content = message };

    //        //throw new HttpResponseException(sipi);

    //      //  return sipi;

    //    }
    //}
}
