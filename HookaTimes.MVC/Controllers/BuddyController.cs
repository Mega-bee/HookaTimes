using HookaTimes.BLL.IServices;
using HookaTimes.BLL.Utilities;
using HookaTimes.BLL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HookaTimes.MVC.Controllers
{
    [Authorize(Roles = "User")]

    public class BuddyController : Controller
    {
        private readonly IAuthBO _auth;
        private readonly IHookaBuddyBL _buddy;

        public BuddyController(IAuthBO auth, IHookaBuddyBL buddy)
        {
            _auth = auth;
            _buddy = buddy;
        }

        [HttpGet]
        public async Task<IActionResult> BuddyProfile(int id)
        {
            ResponseModel Buddy = new ResponseModel();

            if (id != 0)
            {
                Buddy = await _buddy.GetBuddy(id, Request);
            }
            return View(Buddy.Data.Data);
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> InviteBuddy(SendInvitation_VM model)
        {
            string UserId = Tools.GetClaimValue(HttpContext, ClaimTypes.NameIdentifier);
            int userBuddyId = await _auth.GetBuddyById(UserId);
            if (userBuddyId == 0)
            {
                return View();
            }
            ResponseModel res = await _buddy.InviteBuddy(userBuddyId, model);
            return Ok(res);
        }
    }
}
