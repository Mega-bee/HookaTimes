using HookaTimes.BLL.IServices;
using HookaTimes.BLL.Utilities;
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

        public IActionResult Index()
        {
            return View();
        }

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
    }
}
