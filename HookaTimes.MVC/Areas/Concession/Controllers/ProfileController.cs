using Microsoft.AspNetCore.Mvc;

namespace HookaTimes.MVC.Areas.Concession.Controllers
{
    [Area("Concession")]
    public class ProfileController : Controller
    {
        public IActionResult ProfileDetails()
        {
            return View("~/Areas/Concession/Views/Pages/ProfileDetails/ProfileDetails.cshtml");
        }
    }
}
