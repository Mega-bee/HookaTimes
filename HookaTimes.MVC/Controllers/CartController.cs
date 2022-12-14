using HookaTimes.BLL.IServices;
using HookaTimes.BLL.Utilities;
using HookaTimes.BLL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HookaTimes.MVC.Controllers
{
    [Authorize(Roles = "User")]
    public class CartController : Controller
    {
        private readonly ICartBL _cartBL;
        private readonly IAuthBO _auth;

        public CartController(ICartBL cartBL, IAuthBO auth)
        {
            _cartBL = cartBL;
            _auth = auth;
        }

        [AllowAnonymous]
        public async Task<IActionResult> AddToCart([FromForm] int productId, [FromForm]int quantity)
        {
            try
            {
                int userBuddyId = 0;
                var identity = HttpContext.User.Identity as ClaimsIdentity;
              
                if (identity!.IsAuthenticated)
                {
                    string UserId = Tools.GetClaimValue(HttpContext, ClaimTypes.NameIdentifier);
                    userBuddyId = await _auth.GetBuddyById(UserId);
                    return Ok(await _cartBL.AddToCart(userBuddyId, quantity, productId));
                }
                string cartSessionId = Request.Cookies["CartSessionId"]!;
                if (string.IsNullOrEmpty(cartSessionId))
                {
                    cartSessionId = Guid.NewGuid().ToString();
                    CookieOptions cookieOptions = new CookieOptions()
                    {
                           MaxAge = new TimeSpan(100,100,100,100)
                    };
                    Response.Cookies.Append("CartSessionId", cartSessionId, cookieOptions);

                }

                return Ok(await _cartBL.AddToCartCookies(cartSessionId, productId, quantity));
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }

        [HttpDelete]
        [AllowAnonymous]
        public async Task<IActionResult> RemoveItemFromCart([FromForm] int productId)
        {
            try
            {
                int userBuddyId = 0;
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity!.IsAuthenticated)
                {
                    string UserId = Tools.GetClaimValue(HttpContext, ClaimTypes.NameIdentifier);
                    userBuddyId = await _auth.GetBuddyById(UserId);

                }
                string cartSessionId = Request.Cookies["CartSessionId"]!;
                

                return Ok(await _cartBL.RemoveItemFromCart(productId, userBuddyId, cartSessionId));
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            int userBuddyId = 0;
            string cartSessionId = Request.Cookies["CartSessionId"];
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity!.IsAuthenticated)
            {
                string UserId = Tools.GetClaimValue(HttpContext, ClaimTypes.NameIdentifier);
                userBuddyId = await _auth.GetBuddyById(UserId);

            }
          CartSummary_VM cartSummary = await _cartBL.GetCartSummaryMVC(userBuddyId,cartSessionId);
            return View(cartSummary);
        }

        [AllowAnonymous] 
        public async Task<IActionResult> GetCartDropdown()
        {
            return ViewComponent("CartDropdown");
        }

        [AllowAnonymous]
        public async Task<IActionResult> UpdateCart([FromForm]List<UpdateCartItem_VM> items)
        {
            int userBuddyId = 0;
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity!.IsAuthenticated)
            {
                string UserId = Tools.GetClaimValue(HttpContext, ClaimTypes.NameIdentifier);
                userBuddyId = await _auth.GetBuddyById(UserId);
            }
            string cartSessionId = Request.Cookies["CartSessionId"]!;
            return Ok(await _cartBL.UpdateCart(items, userBuddyId, cartSessionId));
        }


    }
}
