using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HookaTimes.BLL.Utilities.ActionFilters;

namespace HookaTimes.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public class APIBaseController : ControllerBase
    {

    }
}
