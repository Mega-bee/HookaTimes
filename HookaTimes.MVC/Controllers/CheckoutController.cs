using HookaTimes.BLL.IServices;
using HookaTimes.BLL.Utilities;
using HookaTimes.BLL.ViewModels;
using HookaTimes.BLL.ViewModels.Website;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HookaTimes.MVC.Controllers
{
    [Authorize(Roles = "User")]
    public class CheckoutController : Controller
    {
        private readonly ICartBL _cartBL;
        private readonly IAuthBO _auth;
        private readonly IOrderBL _orderBL;

        public CheckoutController(ICartBL cartBL, IAuthBO auth, IOrderBL orderBL)
        {
            _cartBL = cartBL;
            _auth = auth;
            _orderBL = orderBL;
        }

        public async Task<IActionResult> Index()
        {
            int userBuddyId = 0;
            string UserId = Tools.GetClaimValue(HttpContext, ClaimTypes.NameIdentifier);
            userBuddyId = await _auth.GetBuddyById(UserId);
            CartSummary_VM cartSummary = await _cartBL.GetCartSummaryMVC(userBuddyId, null);
            Checkout_VM model = new Checkout_VM()
            {
                Address = new BuddyProfileAddressVM(),
                CartSummary = cartSummary

            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(BuddyProfileAddressVM address)
        {
            int userBuddyId = 0;
            string UserId = Tools.GetClaimValue(HttpContext, ClaimTypes.NameIdentifier);
            userBuddyId = await _auth.GetBuddyById(UserId);
            var res = await _orderBL.PlaceOrder(userBuddyId,0,address);
            return RedirectToAction("Index","Home");
        }
    }
}
