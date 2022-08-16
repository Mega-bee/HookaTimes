using HookaTimes.BLL.IServices;
using HookaTimes.BLL.Utilities;
using HookaTimes.BLL.ViewModels;
using HookaTimes.BLL.ViewModels.Website;
using HookaTimes.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace HookaTimes.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICuisineBL _cuisineBl;
        private readonly IHookaPlaceBL _hookaPlaceBL;
        private readonly IHookaBuddyBL _hookaBuddyBL;
        private readonly IProductBL _productBL;
        private readonly IAuthBO _auth;

        public HomeController(ILogger<HomeController> logger, ICuisineBL cuisineBl, IHookaPlaceBL hookaPlaceBL, IHookaBuddyBL hookaBuddyBL, IProductBL productBL, IAuthBO auth)
        {
            _logger = logger;
            _cuisineBl = cuisineBl;
            _hookaPlaceBL = hookaPlaceBL;
            _hookaBuddyBL = hookaBuddyBL;
            _productBL = productBL;
            _auth = auth;
        }

        [Authorize(Roles = "User")]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "User")]
        [AllowAnonymous]

        public async Task<IActionResult> HookaBuddies()
        {
            int userBuddyId = 0;
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity!.IsAuthenticated)
            {
                string UserId = Tools.GetClaimValue(HttpContext, ClaimTypes.NameIdentifier);
                userBuddyId = await _auth.GetBuddyById(UserId);
            }
            List<Buddy_VM> buddies = await _hookaBuddyBL.GetBuddiesMVC(Request, userBuddyId);
            return View(buddies);
        }


        [Authorize(Roles = "User")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> HookaBuddiesSearch([FromQuery] int sortBy, [FromQuery] int filterBy)
        {
            int userBuddyId = 0;
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity!.IsAuthenticated)
            {
                string UserId = Tools.GetClaimValue(HttpContext, ClaimTypes.NameIdentifier);
                userBuddyId = await _auth.GetBuddyById(UserId);
            }
            return ViewComponent("BuddiesSearchResult", new { userBuddyId, sortBy, filterBy });
        }


        [Authorize(Roles = "User")]
        [AllowAnonymous]

        public async Task<IActionResult> HookaProducts()
        {
            int userBuddyId = 0;
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity!.IsAuthenticated)
            {
                string UserId = Tools.GetClaimValue(HttpContext, ClaimTypes.NameIdentifier);
                userBuddyId = await _auth.GetBuddyById(UserId);
            }
            string wishlistSessionId = Request.Cookies["WishlistSessionId"]!;
            List<Product_VM> products = await _productBL.GetAllProductsMVC(userBuddyId, Request, wishlistSessionId);
            return View(products);
        }

        public async Task<IActionResult> HookaPlaces()
        {
            List<Cuisine_VM> cuisines = await _cuisineBl.GetCuisinesMVC();
            var res = await _hookaPlaceBL.GetHookaPlaces(Request);
            List<HookaPlaces_VM> places = (List<HookaPlaces_VM>)res.Data.Data;
            PlacesPage_VM model = new PlacesPage_VM()
            {
                Cuisines = cuisines,
                Places = places
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult HookaPlacesSearch([FromQuery] List<int> cuisines, [FromQuery] int sortBy)
        {

            return ViewComponent("PlacesSearchResult", new { cuisines, sortBy });
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


            return PartialView("~/Views/Shared/Ecommerce/_QuickView.cshtml", null);
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



        //public IActionResult EditProfile()
        //{
        //    return View();
        //}


        //public IActionResult OrderHistory()
        //{
        //    return View();
        //}
        //public IActionResult Addresses()
        //{
        //    return View();
        //}



        public async Task<IActionResult> Product(int id)
        {
            string wishlistSessionId = Request.Cookies["WishlistSessionId"]!;
            int BuddyId = 0;
            string UserId = Tools.GetClaimValue(HttpContext, ClaimTypes.NameIdentifier);
            BuddyId = await _auth.GetBuddyById(UserId);
            ViewHookaProduct_VM prod = await _productBL.GetCategoryProductsMVC(id, wishlistSessionId, BuddyId);
            return View(prod);
        }

        public IActionResult Place()
        {
            return View();
        }

        //public IActionResult Invitations()
        //{
        //    return View();
        //}

        //public IActionResult InvitationPlace()
        //{
        //    return View();
        //}


        public IActionResult Buddy()
        {
            return View();
        }


    }
}