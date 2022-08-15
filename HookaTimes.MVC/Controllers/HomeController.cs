using HookaTimes.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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

        public IActionResult HookaBuddies()
        {
            return View();
        }

        public IActionResult HookaProducts()
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

        public IActionResult Careers()
        {
            return View();
        }

        public IActionResult BecomePartner()
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

        public IActionResult ContactUs()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }



        public IActionResult EditProfile()
        {
            return View();
        }


        public IActionResult OrderHistory()
        {
            return View();
        }
        public IActionResult Addresses()
        {
            return View();
        }

        public IActionResult Password()
        {
            return View();
        }

        public IActionResult Product()
        {
            return View();
        }

        public IActionResult Place()
        {
            return View();
        }

        public IActionResult Invitations()
        {
            return View();
        }

        public IActionResult InvitationPlace()
        {
            return View();
        }


        public IActionResult Buddy()
        {
            return View();
        }
    }
}