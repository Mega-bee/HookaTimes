using Microsoft.AspNetCore.Mvc;

namespace HookaTimes.MVC.Areas.Distributor.Controllers
{
    [Area("Distributor")]
    public class SalesController : Controller
    {

        public IActionResult Index()
        {
            return View("~/Areas/Distributor/Views/Pages/Sales/Index.cshtml");
        }
        public IActionResult Order(int id)
        {
            return View("~/Areas/Distributor/Views/Pages/Sales/Order.cshtml");
        }
    }
}
