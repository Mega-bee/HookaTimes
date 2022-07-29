using Microsoft.AspNetCore.Mvc;

namespace HookaTimes.MVC.Areas.Restaurant.Controllers
{
    [Area("Restaurant")]
    public class RestaurantProfileController : Controller
    {

        public IActionResult EditProfile()
        {
            return View("~/Areas/Restaurant/Views/Pages/RestaurantProfile/RestaurantProfile.cshtml");
        }


        public IActionResult Albums()
        {
            return View("~/Areas/Restaurant/Views/Pages/RestaurantProfile/Albums.cshtml");
        }
    }
}
