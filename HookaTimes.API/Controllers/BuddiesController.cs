using HookaTimes.BLL.IServices;
using HookaTimes.BLL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
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
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            int userBuddyId = Convert.ToInt32(identity.FindFirst("BuddyID").Value);
            return Ok(await _hookaBuddyBL.GetBuddies(Request,userBuddyId, uid));
        }


        [HttpGet("{buddyId}")]
        public async Task<IActionResult> GetBuddyProfile([FromRoute] int buddyId)
        {
            return Ok(await _hookaBuddyBL.GetBuddy(buddyId, Request));
        }



        [HttpPost]
        public async Task<IActionResult> InviteBuddy([FromForm] SendInvitation_VM model)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            int userBuddyId = Convert.ToInt32(identity.FindFirst("BuddyID").Value);
            return Ok(await _hookaBuddyBL.InviteBuddy(userBuddyId, model));
        }
    }
}
