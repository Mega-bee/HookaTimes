using HookaTimes.BLL.IServices;
using HookaTimes.BLL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace HookaTimes.MVC.Views.Home.Components.Places
{
    public class PlacesViewComponent : ViewComponent
    {
        private readonly IHookaPlaceBL _hookaPlaceBL;

        public PlacesViewComponent(IHookaPlaceBL hookaPlaceBL)
        {
            _hookaPlaceBL = hookaPlaceBL;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var res = await _hookaPlaceBL.GetHookaPlaces(Request);
            List<HookaPlaces_VM> places = (List<HookaPlaces_VM>)res.Data.Data;
            return View(places.Take(6).ToList());
        }
    }
}
