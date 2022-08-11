using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using HookaTimes.MVC.Models;
using HookaTimes.BLL.IServices;
using HookaTimes.BLL.ViewModels;
using System.Security.Claims;
using HookaTimes.BLL.ViewModels.Frontend;
using Microsoft.AspNetCore.Authorization;

namespace HookaTimes.MVC.Controllers
{
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHookaPlaceBL _hookaPlaceBL;
        private readonly IHookaBuddyBL _hookaBuddyBL;
        private readonly IProductBL _productBL;

        public HomeController(ILogger<HomeController> logger, IHookaPlaceBL hookaPlaceBL, IHookaBuddyBL hookaBuddyBL, IProductBL productBL)
        {
            _logger = logger;
            _hookaPlaceBL = hookaPlaceBL;
            _hookaBuddyBL = hookaBuddyBL;
            _productBL = productBL;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            int userBuddyId = 0;
            string uid = "";
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if(identity!.IsAuthenticated)
            {
                uid = User.FindFirst(ClaimTypes.NameIdentifier)!.Value!;

                userBuddyId = Convert.ToInt32(identity!.FindFirst("BuddyID")!.Value);
            }
       
            var placesRes = await _hookaPlaceBL.GetHookaPlaces(Request);
            var buddiesRes = await _hookaBuddyBL.GetBuddies(Request,userBuddyId,uid);
            var productsRes = await _productBL.GetAllCategories(Request);
            List<HookaPlaces_VM> places = (List<HookaPlaces_VM>)placesRes.Data.Data;
            List<HookaBuddy_VM> buddies = (List<HookaBuddy_VM>)buddiesRes.Data.Data;
            List<ProductCategories_VM> products = (List<ProductCategories_VM>)productsRes.Data.Data;
            HomePage_VM dto = new HomePage_VM()
            {
                 Buddies = buddies.Take(6).ToList(),
                  Categories = products.Take(6).ToList(),
                   Places = places.Take(6).ToList(),
            };
            return View(dto);
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
    }
}