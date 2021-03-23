using ActivityCalculator.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityCalculator.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class BaseController : Controller
    {
        protected ActionResult<ResponseVm<T>> ApiResponse<T>(T data)
        {
            return Ok(new ResponseVm<T>(data));
        }
    }
}
