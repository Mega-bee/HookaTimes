using HookaTimes.BLL.IServices;
using HookaTimes.BLL.Service;
using HookaTimes.BLL.Utilities;
using HookaTimes.BLL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HookaTimes.MVC.Controllers
{
    [Authorize(Roles = "User")]
    public class WishlistController : Controller
    {
        private readonly IAuthBO _auth;
        private readonly IWishlistBL _wishlistBL;

        public WishlistController(IAuthBO auth, IWishlistBL wishlistBL)
        {
            _auth = auth;
            _wishlistBL = wishlistBL;
        }

        public async Task<IActionResult> Index()
        {
            int userBuddyId = 0;
            string wishlistSessionId = "";
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity!.IsAuthenticated)
            {
                string UserId = Tools.GetClaimValue(HttpContext, ClaimTypes.NameIdentifier);
                userBuddyId = await _auth.GetBuddyById(UserId);

            }
            wishlistSessionId = Request.Cookies["WishlistSessionId"]!;
            List<Wishlist_VM> items = await _wishlistBL.GetWishlist(userBuddyId,wishlistSessionId);
            return View(items);
        }


        [AllowAnonymous]
        public async Task<IActionResult> AddToWishlist([FromForm] int productId)
        {
            int userBuddyId = 0;
            string wishlistSessionId = "";
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity!.IsAuthenticated)
            {
                string UserId = Tools.GetClaimValue(HttpContext, ClaimTypes.NameIdentifier);
                userBuddyId = await _auth.GetBuddyById(UserId);
               
            }
             wishlistSessionId = Request.Cookies["WishlistSessionId"]!;
            if (string.IsNullOrEmpty(wishlistSessionId))
            {
                wishlistSessionId = Guid.NewGuid().ToString();
                CookieOptions cookieOptions = new CookieOptions()
                {
                    MaxAge = new TimeSpan(100, 100, 100, 100)
                };
                Response.Cookies.Append("WishlistSessionId", wishlistSessionId, cookieOptions);

            }

            return Ok(await _wishlistBL.AddToWishlist(productId, wishlistSessionId, userBuddyId));
        }

        [HttpDelete]
        [AllowAnonymous]
        public async Task<IActionResult> RemoveItemFromWishlist([FromForm] int productId)
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
                string wishlistSessionId = Request.Cookies["WishlistSessionId"]!;


                return Ok(await _wishlistBL.RemoveItemFromWishlist(productId, userBuddyId, wishlistSessionId));
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}
