using HookaTimes.BLL.IServices;
using HookaTimes.BLL.Utilities;
using HookaTimes.BLL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HookaTimes.MVC.Views.Home.Components.Places
{
    public class PlacesViewComponent : ViewComponent
    {
        private readonly IHookaPlaceBL _hookaPlaceBL;
        private readonly IAuthBO _auth;

        public PlacesViewComponent(IHookaPlaceBL hookaPlaceBL, IAuthBO auth)
        {
            _hookaPlaceBL = hookaPlaceBL;
            _auth = auth;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            int userBuddyId = 0;
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity!.IsAuthenticated)
            {
                string UserId = Tools.GetClaimValue(HttpContext, ClaimTypes.NameIdentifier);
                userBuddyId = await _auth.GetBuddyById(UserId);
            }
            var res = await _hookaPlaceBL.GetHookaPlaces(Request,userBuddyId);
            List<HookaPlaces_VM> places = (List<HookaPlaces_VM>)res.Data.Data;
            return View(places.Take(8).ToList());
        }
    }
}
