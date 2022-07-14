using Microsoft.AspNetCore.Mvc;

namespace HookaTimes.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HumanResourcesController : Controller
    {
        public IActionResult HookaTeamStaff([FromQuery] string pageName)
        {
            ViewBag.PageName = pageName;
            return View("~/Areas/Admin/Views/Pages/HumanResources/HookaTeamStaff.cshtml");
        }

        public IActionResult HookaTeamStaffMember()
        {
            return View("~/Areas/Admin/Views/Pages/HumanResources/HookaTimesStaffMember.cshtml");
        }

        public IActionResult HookaTimesTeam()
        {
            return View("~/Areas/Admin/Views/Pages/HumanResources/HookaTimesTeam.cshtml");
        }
    }
}
