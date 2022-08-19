using HookaTimes.BLL.Utilities.ActionFilters;
using Microsoft.AspNetCore.Mvc;

namespace HookaTimes.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public class APIBaseController : ControllerBase
    {

    }
}
