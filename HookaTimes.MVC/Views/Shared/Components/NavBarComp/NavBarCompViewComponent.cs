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
        private readonly IWishlistBL _wishlistBL;

        public NavBarCompViewComponent(IAuthBO auth, ICartBL cartBL, IWishlistBL wishlistBL)
        {
            _auth = auth;
            _cartBL = cartBL;
            _wishlistBL = wishlistBL;
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
            string wishlistSessionId = Request.Cookies["WishlistSessionId"]!;
            NavBuddy_VM buddy = await _auth.GetNavBuddyProfile(UserId);
            CartSummary_VM cartSummary = await _cartBL.GetCartSummaryMVC(userBuddyId, cartSessionId);
            int wishlistCount = await _wishlistBL.GetWishlistCount(userBuddyId, wishlistSessionId);
            EmailSignInMVC_VM model = new EmailSignInMVC_VM();



            ViewBag.Buddy = buddy;
            ViewBag.CartSummary = cartSummary;
            ViewBag.WishlistCount = wishlistCount;


            return View(model);
        }
    }
}


