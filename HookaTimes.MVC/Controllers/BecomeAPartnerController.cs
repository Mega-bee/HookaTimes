using Microsoft.AspNetCore.Mvc;

namespace HookaTimes.MVC.Controllers
{
    public class BecomeAPartnerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> BecomeAPartner()
        {
            return View();
        }
    }
}
