using Microsoft.AspNetCore.Mvc;

namespace HookaTimes.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DistributorsController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Areas/Admin/Views/Pages/Distributors/Index.cshtml");
        }

        public IActionResult Distributor()
        {
            return View("~/Areas/Admin/Views/Pages/Distributors/Distributor.cshtml");
        }
    }
}
