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

        public IActionResult Details()
        {
            return View("~/Areas/Admin/Views/Pages/Sales/Details.cshtml");
        }
    }
}
