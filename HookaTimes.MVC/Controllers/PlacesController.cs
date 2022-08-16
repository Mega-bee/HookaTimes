using HookaTimes.BLL.IServices;
using HookaTimes.BLL.Service;
using HookaTimes.BLL.Utilities;
using HookaTimes.BLL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HookaTimes.MVC.Controllers
{
    public class PlacesController : Controller
    {
        private readonly IHookaPlaceBL _hookaPlaceBL;
        private readonly IAuthBO _auth;

        public PlacesController(IHookaPlaceBL hookaPlaceBL, IAuthBO auth)
        {
            _hookaPlaceBL = hookaPlaceBL;
            _auth = auth;
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
            return View(place);
        }
    }
}
