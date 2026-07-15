using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using UserControl.Core.Constants;
using UserControl.Core.Exeptions;

namespace UserControl.Api.CustomExceptionMiddleware
{
    public class ExceptionMiddleware
    {

        private readonly RequestDelegate _next;
       // private readonly ILoggerManager _logger;

       // public ExceptionMiddleware(RequestDelegate next, ILoggerManager logger)
        public ExceptionMiddleware(RequestDelegate next)
        {
           // _logger = logger;
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

                //esta si se quiere guardar mas informacion
                //_logger.LogError($"Something went wrong: {ex}");

                //ESTA ES PARA GUARDAR UN LOG DE ERRORES
                //esta sipi
              //  _logger.LogError($"Something went wrong: {ErrorCodes.msgUserNotFound}");

                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            int codeError = 0;
            bool band;
            context.Response.ContentType = "application/json";

            if (exception.GetType() == typeof(BusinessExceptions))
            {
                band = int.TryParse(exception.Message, out codeError);

                context.Response.StatusCode = band ? codeError : int.Parse(ErrorCodes.defaultCode); //int.Parse(exception.Message); // 1;
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            return context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = codeError != 0 ? ErrorCodes.GetErrorMessage(codeError.ToString()) : "Internal Server Error from the custom middleware."
                
            }.ToString());
        }

    }
}
