using Microsoft.AspNetCore.Mvc;

namespace HookaTimes.MVC.Areas.Restaurant.Controllers
{
    [Area("Restaurant")]
    public class DashboardsController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Areas/Restaurant/Views/Pages/Dashboards/Index.cshtml");
        }
    }
}
