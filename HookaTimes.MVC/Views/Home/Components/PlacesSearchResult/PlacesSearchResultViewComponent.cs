using HookaTimes.BLL.IServices;
using HookaTimes.BLL.Utilities;
using HookaTimes.BLL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HookaTimes.MVC.Views.Home.Components.PlacesSearchResult
{
    public class PlacesSearchResultViewComponent : ViewComponent
    {
        private readonly IHookaPlaceBL _hookaPlaceBL;
        private readonly IAuthBO _auth;

        public PlacesSearchResultViewComponent(IHookaPlaceBL hookaPlaceBL, IAuthBO auth)
        {
            _hookaPlaceBL = hookaPlaceBL;
            _auth = auth;
        }

        public async Task<IViewComponentResult> InvokeAsync(int take = 0, List<int>? cuisines = null, int sortBy = 0)
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
                List<HookaPlaces_VM> places = await _hookaPlaceBL.GetHookaPlacesMVC(Request, take,userBuddyId, cuisines, sortBy);
                return View(places);
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }
    }
}
