using HookaTimes.BLL.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HookaTimes.API.Controllers
{
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]

    public class BuddiesController : APIBaseController
    {
        private readonly IHookaBuddyBL _hookaBuddyBL;

        public BuddiesController(IHookaBuddyBL hookaBuddyBL)
        {
            _hookaBuddyBL = hookaBuddyBL;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBuddies()
        {
            string uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(await _hookaBuddyBL.GetBuddies(Request,uid));
        }

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetBuddy([FromRoute] int id)
        //{
        //    return Ok(await _hookaBuddyBL.GetB(Request, id));
        //}

        //[HttpPut("{placeId}")]
        //public async Task<IActionResult> ToggleFavorite([FromRoute] int placeId)
        //{
        //    string uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        //    return Ok(await _hookaPlaceBL.AddToFavorites(uid, placeId));
        //}
    }
}
