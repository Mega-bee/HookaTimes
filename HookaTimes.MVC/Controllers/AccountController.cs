using HookaTimes.BLL.IServices;
using HookaTimes.BLL.Utilities;
using HookaTimes.BLL.ViewModels;
using HookaTimes.BLL.ViewModels.Website;
using HookaTimes.DAL.Data;
using HookaTimes.DAL.HookaTimesModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Claims;
using System.Text;

namespace HookaTimes.MVC.Controllers
{
    [Authorize]

    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAuthBO _auth;
        private readonly IInvitationBL _inv;


        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
          RoleManager<IdentityRole> roleManager, IAuthBO auth, IInvitationBL inv)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _auth = auth;
            _inv = inv;
        }
        public IActionResult Index()
        {

            return View();
        }

        #region Sign UP 

        [HttpGet]
        [AllowAnonymous]
        public IActionResult SignUp()
        {
            //ViewData["ReturnUrl"] = returnurl;
            //EmailSignUpMVC_VM registerVm = new EmailSignUpMVC_VM();
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(EmailSignUpMVC_VM model)
        {
            //ViewData["ReturnUrl"] = returnurl;
            //returnurl = returnurl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {

                string cartSessionId = Request.Cookies["CartSessionId"]!;
                string wishlistSessionId = Request.Cookies["WishlistSessionId"]!;
                IdentityResult res = await _auth.SignUpWithEmailMVC(model, wishlistSessionId, cartSessionId);



                if (res == null)
                {
                    return View(model);

                }

                //Create buddy Profile
                BuddyProfile buddy = await _auth.CreateBuddyProfileMVC(model);

                //Get the Identity User Profile so it can get its claims and roles
                ApplicationUser newUser = await _userManager.FindByEmailAsync(model.Email);

                var roles = await _userManager.GetRolesAsync(newUser);

                var claims = Tools.GenerateClaimsMVC(newUser, roles, buddy);

                ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);


                ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                ////Set current principal
                Thread.CurrentPrincipal = principal;

                User.AddIdentity(identity);


                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                    new AuthenticationProperties());

                await _signInManager.SignInAsync(newUser, isPersistent: true);

                return RedirectToAction("Index", "Home");
                // return LocalRedirect(returnurl);

            }

            return View(model);

            //if (ModelState.IsValid)
            //{

            //    return LocalRedirect(returnurl);

            //}
            //return View(model);
        }
        #endregion


        #region Sign IN
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(EmailSignInMVC_VM model, string returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            returnurl = returnurl ?? Url.Content("~/");
            //if (ModelState.IsValid)
            //{
            string cartSessionId = Request.Cookies["CartSessionId"]!;
            string wishlistSessionId = Request.Cookies["WishlistSessionId"]!;

            ClaimsIdentity identity = await _auth.EmailSignInMVC(model, wishlistSessionId, cartSessionId);
            if (identity == null)
            {
                return LocalRedirect(returnurl);

            }
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            //Set current principal
            Thread.CurrentPrincipal = principal;

            User.AddIdentity(identity);

            TempData["success"] = "Please Login Again";

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                new AuthenticationProperties());
            return LocalRedirect(returnurl);
            //}

            //return View(model);


        }
        #endregion



        #region Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
            //return RedirectToAction(nameof(HomeController.Index), nameof(HomeController));
        }
        #endregion


        #region Password
        [Authorize(Roles = "User")]
        [HttpGet]
        public IActionResult Password()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Password(PasswordMVC_VM model)
        {
            //temp
            if (ModelState.IsValid)
            {
                string UID = Tools.GetClaimValue(HttpContext, ClaimTypes.NameIdentifier);

                var user = await _userManager.FindByIdAsync(UID);

                if (user == null)
                {
                    TempData["error"] = "User Was Not Found!";

                    return View(model);
                }
                if (!await _userManager.CheckPasswordAsync(user, model.CurrentPassword))
                {

                    TempData["error"] = "Passwords is same as old!";
                    return View(model);

                }
                if (model.NewPassword == model.CurrentPassword)
                {
                    TempData["error"] = "Passwords is same as old!";

                    return View(model);

                }
                if (model.NewPassword != model.ConfirmPassword)
                {
                    TempData["error"] = "Passwords don't match!";

                    return View(model);

                }
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var encodedToken = Encoding.UTF8.GetBytes(token);
                var validToken = WebEncoders.Base64UrlEncode(encodedToken);
                var decodedToken = WebEncoders.Base64UrlDecode(validToken);
                string normalToken = Encoding.UTF8.GetString(decodedToken);

                var result = await _userManager.ResetPasswordAsync(user, normalToken, model.NewPassword);
                if (result.Succeeded)
                {
                    await _signInManager.SignOutAsync();
                    TempData["success"] = "Please Login Again";
                    //return Ok(new { message = "Password has been reset succesfully" });
                    return RedirectToAction("Index", "Home");
                }

                return View(model);
            }
            return View(model);
        }

        #endregion

        #region OrderHistory
        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> OrderHistory()
        {
            string UserId = Tools.GetClaimValue(HttpContext, ClaimTypes.NameIdentifier);

            int userBuddyId = await _auth.GetBuddyById(UserId);

            List<OrderHistoryMVC_VM> orderHistory = Array.Empty<OrderHistoryMVC_VM>().ToList();
            if (userBuddyId != 0)
            {
                orderHistory = await _auth.GetOrderHistoryMVC(userBuddyId);
            }
            return View(orderHistory);
        }
        #endregion


        #region Invitations

        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<IActionResult> Invitations()
        {

            string UserId = Tools.GetClaimValue(HttpContext, ClaimTypes.NameIdentifier);

            int userBuddyId = await _auth.GetBuddyById(UserId);
            ResponseModel invitations = new ResponseModel();
            ResponseModel invitationsSent = new ResponseModel();

            if (userBuddyId != 0)
            {

                invitations = await _inv.GetRecievedInvitations(Request, userBuddyId);

                invitationsSent = await _inv.GetSentInvitations(Request, userBuddyId);
            }
            ViewBag.InvitationsSent = invitationsSent.Data.Data;
            return View(invitations.Data.Data);
        }


        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> SetInvitationStatusMVC([FromForm] int statusId, [FromForm] int invitationId)
        {
            return Ok(await _inv.SetInvitationStatus(statusId, invitationId));


        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<IActionResult> InvitationPlace([FromRoute] int id)
        {
            string UserId = Tools.GetClaimValue(HttpContext, ClaimTypes.NameIdentifier);
            int userBuddyId = await _auth.GetBuddyById(UserId);

            ResponseModel InvitationPlace = new ResponseModel();


            if (userBuddyId != 0)
            {
                InvitationPlace = await _inv.GetPlaceInvitations(Request, id, userBuddyId);
            }
            return View(InvitationPlace.Data.Data);
        }
        #endregion

        //#region Addresses
        //[Authorize(Roles = "User")]
        //[HttpGet]
        //public IActionResult Addresses()
        //{
        //    return View();
        //}


        //[Authorize(Roles = "User")]
        //[HttpGet]
        //public IActionResult CreateAddress()
        //{
        //    return View();
        //}



        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> EditAddress(PasswordMVC_VM model)
        //{
        //    //temp
        //    if (ModelState.IsValid)
        //    {

        //    }
        //    return View(model);
        //}


        //#endregion



    }
}
