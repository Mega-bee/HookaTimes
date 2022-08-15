using HookaTimes.BLL.IServices;
using HookaTimes.BLL.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HookaTimes.MVC.Views.Home.Components.Buddies
{
    public class BuddiesViewComponent : ViewComponent
    {
        private readonly IHookaBuddyBL _hookaBuddyBl;
        private readonly IAuthBO _auth;

        public BuddiesViewComponent(IHookaBuddyBL hookaBuddyBl, IAuthBO auth)
        {
            _hookaBuddyBl = hookaBuddyBl;
            _auth = auth;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            //int userBuddyId = 0;
            //var identity = HttpContext.User.Identity as ClaimsIdentity;
            //if (identity!.IsAuthenticated)
            //{
            //    userBuddyId = Convert.ToInt32(identity.FindFirst("BuddyID")?.Value);
            //}
            int BuddyId = 0;
            string UserId = Tools.GetClaimValue(HttpContext, ClaimTypes.NameIdentifier);
            BuddyId = await _auth.GetBuddyById(UserId);
            var items = await _hookaBuddyBl.GetBuddiesMVC(Request, BuddyId, 6);
            return View(items);
        }
    }
}
