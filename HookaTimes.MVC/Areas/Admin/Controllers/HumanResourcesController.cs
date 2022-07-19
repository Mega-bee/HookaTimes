using Microsoft.AspNetCore.Mvc;

namespace HookaTimes.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HumanResourcesController : Controller
    {
        public IActionResult HookaTimesStaff()
        {
            return View("~/Areas/Admin/Views/Pages/HumanResources/HookaTimesStaff.cshtml");
        }
    }
}
