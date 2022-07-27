using Microsoft.AspNetCore.Mvc;

namespace HookaTimes.MVC.Areas.DeliveryCenter.Controllers
{
    [Area("DeliveryCenter")]
    public class DashboardsController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Areas/DeliveryCenter/Views/Pages/Dashboards/Index.cshtml");
        }
    }
}
