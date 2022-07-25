using Microsoft.AspNetCore.Mvc;

namespace HookaTimes.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SalesController : Controller
    {

        public IActionResult Index()
        {
            return View("~/Areas/Admin/Views/Pages/Sales/Index.cshtml");
        }
        public IActionResult Order(int id)
        {
            return View("~/Areas/Admin/Views/Pages/Sales/Order.cshtml");
        }
    }
}
