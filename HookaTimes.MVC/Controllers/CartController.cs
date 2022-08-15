using HookaTimes.BLL.IServices;
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

        public CartController(ICartBL cartBL)
        {
            _cartBL = cartBL;
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
                    userBuddyId = Convert.ToInt32(identity.FindFirst("BuddyID")!.Value);
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
                    userBuddyId = Convert.ToInt32(identity.FindFirst("BuddyID")!.Value);
                  
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
                userBuddyId = Convert.ToInt32(identity.FindFirst("BuddyID")!.Value);
               
            }
          CartSummary_VM cartSummary = await _cartBL.GetCartSummaryMVC(userBuddyId,cartSessionId);
            return View(cartSummary);
        } 
    }
}
