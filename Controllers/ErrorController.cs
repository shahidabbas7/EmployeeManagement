using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            this.logger = logger;
        }
        [Route("Error/{statusCode}")]
        public IActionResult actionResulCodeHandler(int statusCode)
        {
            var statuscoderesult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            switch (statusCode)
            {
                case 404:
                    ViewBag.error = "Sorry, the resource you requested can not be found";
                    logger.LogWarning($"404 error ocuured path={statuscoderesult.OriginalPath}" + $"and query {statuscoderesult.OriginalQueryString} causes an error");
                    break;
            }
            return View("NotFound");
        }
        [AllowAnonymous]
        [Route("Error")]
        public IActionResult exceptionhandler()
        {
            //get exception detials
            var exceptiondetial = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            logger.LogError($"the path {exceptiondetial.Path} threw an exception " + $"{exceptiondetial.Error}");
          
            return View("error");
        }
    }
}
