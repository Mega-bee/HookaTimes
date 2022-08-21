using Microsoft.AspNetCore.Mvc;

namespace HookaTimes.MVC.Controllers
{
    public class ContactUs : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
