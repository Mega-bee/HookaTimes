using AspNetCoreHero.ToastNotification.Abstractions;
using HookaTimes.BLL.IServices;
using HookaTimes.BLL.Service;
using HookaTimes.BLL.Utilities;
using HookaTimes.BLL.ViewModels;
using HookaTimes.BLL.ViewModels.Website;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HookaTimes.MVC.Controllers
{
    public class PlacesController : Controller
    {
        private readonly IHookaPlaceBL _hookaPlaceBL;
        private readonly IAuthBO _auth;
        private readonly INotyfService _notyf;

        public PlacesController(IHookaPlaceBL hookaPlaceBL, IAuthBO auth, INotyfService notyf)
        {
            _hookaPlaceBL = hookaPlaceBL;
            _auth = auth;
            _notyf = notyf;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Place(int id)
        {
            int userBuddyId = 0;
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity!.IsAuthenticated)
            {
                string UserId = Tools.GetClaimValue(HttpContext, ClaimTypes.NameIdentifier);
                userBuddyId = await _auth.GetBuddyById(UserId);
            }
            
            var res = await _hookaPlaceBL.GetHookaPlace(Request, userBuddyId, id);
            HookaPlaceInfo_VM place = (HookaPlaceInfo_VM)res.Data.Data;
            PlacePage_VM placePage_VM = new PlacePage_VM()
            {
                Place = place,
                Review = new CreateReview_VM()
            };
            return View(placePage_VM);
        }

        [Authorize(Roles ="User")]

        public async Task<IActionResult> AddToFavorites(int placeId)
        {
            string uid = Tools.GetClaimValue(HttpContext, ClaimTypes.NameIdentifier);
            var res = await _hookaPlaceBL.AddToFavorites(uid, placeId);
            if(res.StatusCode == 200|| res.StatusCode == 201)
            {
                _notyf.Success(res.Data.Message);
            } else
            {
                _notyf.Error(res.ErrorMessage);
            }
            return Ok();
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> AddReview([FromRoute]int id, [FromForm]CreateReview_VM review)
        {
            int userBuddyId = 0;
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity!.IsAuthenticated)
            {
                string UserId = Tools.GetClaimValue(HttpContext, ClaimTypes.NameIdentifier);
                userBuddyId = await _auth.GetBuddyById(UserId);
            }

            var res = await _hookaPlaceBL.AddReview(review, Request, id, userBuddyId);
            if (res.StatusCode == 200 || res.StatusCode == 201)
            {
                _notyf.Success(res.Data.Message);
            }
            else
            {
                _notyf.Error(res.ErrorMessage);
            }
            return Ok(res);
        }
    }
}
