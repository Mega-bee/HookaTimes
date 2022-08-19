using HookaTimes.BLL.IServices;
using HookaTimes.BLL.Utilities;
using HookaTimes.BLL.ViewModels.Website;
using HookaTimes.BLL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HookaTimes.MVC.Views.Shared.Components.CartDropdownViewComponent
{
    public class CartDropdownViewComponent : ViewComponent
    {
        private readonly ICartBL _cartBL;
        private readonly IAuthBO _auth;

        public CartDropdownViewComponent(ICartBL cartBL, IAuthBO auth)
        {
            _cartBL = cartBL;
            _auth = auth;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                string UserId = Tools.GetClaimValue(HttpContext, ClaimTypes.NameIdentifier);
                int userBuddyId = 0;
                var identity = HttpContext.User.Identity as ClaimsIdentity;

                if (identity!.IsAuthenticated)
                {

                    userBuddyId = await _auth.GetBuddyById(UserId);

                }
                string cartSessionId = Request.Cookies["CartSessionId"]!;
                CartSummary_VM cartSummary = await _cartBL.GetCartSummaryMVC(userBuddyId, cartSessionId);



                return View(cartSummary);
            }
            catch (Exception e)
            {

                throw;
            }
           
        }
    }
}
