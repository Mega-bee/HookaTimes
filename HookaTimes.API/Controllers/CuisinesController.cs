using HookaTimes.BLL.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HookaTimes.API.Controllers
{
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]

    public class CuisinesController : APIBaseController
    {
        private readonly ICuisineBL _cuisineBL;

        public CuisinesController(ICuisineBL cuisineBL)
        {
            _cuisineBL = cuisineBL;
        }

        [HttpGet]
        public async Task<IActionResult> GetCuisines()
        {
            return Ok(await _cuisineBL.GetCuisines());
        }
    }
}
