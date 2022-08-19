using AspNetCoreHero.ToastNotification.Abstractions;
using HookaTimes.BLL.IServices;
using HookaTimes.BLL.Utilities;
using HookaTimes.BLL.ViewModels;
using HookaTimes.BLL.ViewModels.Website;
using MessagePack;
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
        private readonly INotyfService _notyf;

        public CheckoutController(ICartBL cartBL, IAuthBO auth, IOrderBL orderBL, INotyfService notyf)
        {
            _cartBL = cartBL;
            _auth = auth;
            _orderBL = orderBL;
            _notyf = notyf;
        }

        public async Task<IActionResult> Index(string returnurl = null)
        {
      
            ViewData["ReturnUrl"] = returnurl;
            returnurl = returnurl ?? Url.Content("~/");
            int userBuddyId = 0;
            string UserId = Tools.GetClaimValue(HttpContext, ClaimTypes.NameIdentifier);
            userBuddyId = await _auth.GetBuddyById(UserId);
            bool hasItemsInCart = await _cartBL.CheckIfProductsInCart(userBuddyId);
            if (!hasItemsInCart)
            {
                return LocalRedirect(returnurl);
            }
            CartSummary_VM cartSummary = await _cartBL.GetCartSummaryMVC(userBuddyId, null);
            Checkout_VM model = new Checkout_VM()
            {
                Address = new BuddyProfileAddressVM(),
                CartSummary = cartSummary

            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(BuddyProfileAddressVM address, string returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            returnurl = returnurl ?? Url.Content("~/");
            int userBuddyId = 0;
            string UserId = Tools.GetClaimValue(HttpContext, ClaimTypes.NameIdentifier);
            userBuddyId = await _auth.GetBuddyById(UserId);
            bool hasItemsInCart = await _cartBL.CheckIfProductsInCart(userBuddyId);
            if(!hasItemsInCart)
            {
                return LocalRedirect(returnurl);
            }
            var res = await _orderBL.PlaceOrder(userBuddyId,0,address);
            if(res.StatusCode == 200)
            {
                _notyf.Success("Your order has been placed.");
            } else
            {
                _notyf.Error("Failed to place order.");
            }
            return RedirectToAction("OrderHistory", "Account");
        }
    }
}
