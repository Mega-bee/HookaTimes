using HookaTimes.BLL.IServices;
using HookaTimes.BLL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HookaTimes.MVC.Views.Home.Components.PlacesSearchResult
{
    public class PlacesSearchResultViewComponent : ViewComponent
    {
        private readonly IHookaPlaceBL _hookaPlaceBL;

        public PlacesSearchResultViewComponent(IHookaPlaceBL hookaPlaceBL)
        {
            _hookaPlaceBL = hookaPlaceBL;
        }

        public async Task<IViewComponentResult> InvokeAsync(int take = 0, List<int>? cuisines = null, int sortBy = 0)
        {
            try
            {
                List<HookaPlaces_VM> places = await _hookaPlaceBL.GetHookaPlacesMVC(Request, take, cuisines, sortBy);
                return View(places);
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }
    }
}
