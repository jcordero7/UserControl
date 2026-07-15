using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UserControl.Api.Controllers
{
   // [Route("api/[controller]")]
    [ApiController]
    public class ErrorController : ControllerBase
    {

        [Route("/error-local-development")]
        [HttpGet]
        public IActionResult ErrorLocalDevelopment(
        [FromServices] IWebHostEnvironment webHostEnvironment)
        {
            if (webHostEnvironment.EnvironmentName != "Development")
            {
                throw new InvalidOperationException(
                    "This shouldn't be invoked in non-development environments.");
            }

            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

            return Problem(
                detail: context.Error.StackTrace,
                title: "quiero por favor un titulo personalizado"); //context.Error.Message);
        }

        [HttpGet]
        [Route("/error")]
        public IActionResult Error() => Problem();

    }
}
