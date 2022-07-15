using Microsoft.AspNetCore.Mvc;

namespace HookaTimes.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HookaBuddiesController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Areas/Admin/Views/Pages/HookaBuddies/Index.cshtml");
        }
    }
}
