using HookaTimes.BLL.IServices;
using HookaTimes.BLL.Utilities;
using HookaTimes.BLL.ViewModels;
using HookaTimes.BLL.ViewModels.Website;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HookaTimes.MVC.Views.Shared.Components.NavBarComp
{
    public class NavBarCompViewComponent : ViewComponent
    {
        private readonly IAuthBO _auth;
        private readonly ICartBL _cartBL;

        public NavBarCompViewComponent(IAuthBO auth, ICartBL cartBL)
        {
            _auth = auth;
            _cartBL = cartBL;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string UserId = Tools.GetClaimValue(HttpContext, ClaimTypes.NameIdentifier);
            int userBuddyId = 0;
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity!.IsAuthenticated)
            {
               
                userBuddyId = await _auth.GetBuddyById(UserId);

            }
            string cartSessionId = Request.Cookies["CartSessionId"]!;
            NavBuddy_VM buddy = await _auth.GetNavBuddyProfile(UserId);
            CartSummary_VM cartSummary = await _cartBL.GetCartSummaryMVC(userBuddyId, cartSessionId);
            EmailSignInMVC_VM model = new EmailSignInMVC_VM();


            
            ViewBag.Buddy = buddy;
            ViewBag.CartSummary = cartSummary;

            return View(model);
        }
    }
}
