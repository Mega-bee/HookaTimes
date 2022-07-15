using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using HookaTimes.MVC.Models;

namespace HookaTimes.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }  
        
        public IActionResult HookaPlaces()
        {
            return View();
        }

        public IActionResult WishList()
        {
            return View();
        }


        public IActionResult Cart()
        {
            return View();
        }   
        
        public IActionResult Checkout()
        {
            return View();
        }

        [HttpGet]
        public ActionResult QuickView()
        {
          

            return PartialView("~/Views/Shared/Ecommerce/_QuickViewPartial.cshtml", null);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}