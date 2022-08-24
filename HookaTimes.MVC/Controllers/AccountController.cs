using AspNetCoreHero.ToastNotification.Abstractions;
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
        private readonly INotyfService _notyf;
        private readonly IHookaPlaceBL _hookaPlaceBL;
        private readonly IHookaBuddyBL _buddy;
        private readonly IOrderBL _orderbl;
        public AccountController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, IAuthBO auth, IInvitationBL inv, INotyfService notyf, IHookaPlaceBL hookaPlaceBL, IHookaBuddyBL buddy, IOrderBL orderbl)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _auth = auth;
            _inv = inv;
            _notyf = notyf;
            _hookaPlaceBL = hookaPlaceBL;
            _buddy = buddy;
            _orderbl = orderbl;
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
                IdentityResult res = await _auth.SignUpWithEmailMVC(model);



                if (res == null)
                {
                    _notyf.Error("SignUp Failed");
                    _notyf.Custom("Email Already Exist", 6, "whitesmoke", "fa fa-gear");

                    return View(model);

                }

                //Create buddy Profile
                BuddyProfile buddy = await _auth.CreateBuddyProfileMVC(model, wishlistSessionId, cartSessionId);
                if (!string.IsNullOrEmpty(cartSessionId))
                {
                    Response.Cookies.Delete("CartSessionId");
                }
                if (!string.IsNullOrEmpty(wishlistSessionId))
                {
                    Response.Cookies.Delete("WishlistSessionId");
                }
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

                _notyf.Success("Welcome to HookaTimes!", 6);
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
                //TempData["error"] = "Check your email and pass";
                _notyf.Error("Invalid Credentials");
                return LocalRedirect(returnurl);

            }
            if (!string.IsNullOrEmpty(cartSessionId))
            {
                Response.Cookies.Delete("CartSessionId");
            }
            if (!string.IsNullOrEmpty(wishlistSessionId))
            {
                Response.Cookies.Delete("WishlistSessionId");
            }
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            //Set current principal
            Thread.CurrentPrincipal = principal;

            User.AddIdentity(identity);

            //TempData["success"] = "Please Login Again";

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                new AuthenticationProperties());

            _notyf.Success("Welcome To HookaTimes!");

            return LocalRedirect(returnurl);
            //}

            //return View(model);


        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            EmailSignInMVC_VM loginmv = new EmailSignInMVC_VM();
            return View(loginmv);
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

                    _notyf.Error("User Was Not Found!", 6);


                    return View(model);
                }
                if (!await _userManager.CheckPasswordAsync(user, model.CurrentPassword))
                {
                    _notyf.Error("Password is same as old!", 6);

                    return View(model);

                }
                if (model.NewPassword == model.CurrentPassword)
                {
                    _notyf.Error("Password is same as old!", 6);

                    return View(model);

                }
                if (model.NewPassword != model.ConfirmPassword)
                {
                    _notyf.Error("Passwords don't match!", 6);


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

                    _notyf.Success("Your password has been changed");
                    _notyf.Custom("Please Login Again with the new password", 5, "whitesmoke", "fa fa-gear");
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


        [Authorize(Roles = "User")]
        public async Task<IActionResult> OrderDetails(int id)
        {
            string UserId = Tools.GetClaimValue(HttpContext, ClaimTypes.NameIdentifier);

            int userBuddyId = await _auth.GetBuddyById(UserId);
            var res = await _orderbl.GetOrder(Request, userBuddyId, id);
            OrderDetails_VM order = (OrderDetails_VM)res.Data.Data;
            return View(order);
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
        public async Task<IActionResult> Favorites()
        {

            string UserId = Tools.GetClaimValue(HttpContext, ClaimTypes.NameIdentifier);

            int userBuddyId = await _auth.GetBuddyById(UserId);

            List<HookaPlaces_VM> favs = await _hookaPlaceBL.GetFavorites(userBuddyId);


            return View(favs);
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


        #region Profile
        public async Task<IActionResult> EditProfile()
        {
            string UserId = Tools.GetClaimValue(HttpContext, ClaimTypes.NameIdentifier);
            int userBuddyId = await _auth.GetBuddyById(UserId);
            if (userBuddyId != 0)
            {
                CompleteProfileMVC_VM res = await _auth.GetProfileMVC(userBuddyId);
                return View(res);
            }
            return View();
        }
        [Authorize(Roles = "User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteProfile(CompleteProfileMVC_VM model)
        {
            string UserId = Tools.GetClaimValue(HttpContext, ClaimTypes.NameIdentifier);
            int userBuddyId = await _auth.GetBuddyById(UserId);
            if (userBuddyId != 0)
            {
                bool res = await _auth.CompleteProfileMVC(model, userBuddyId);
                _notyf.Success("Profile Info Updated", 6);

                return RedirectToAction(nameof(EditProfile), "Account");
            }
            _notyf.Error("Error Profile was not Updated", 6);

            return RedirectToAction(nameof(EditProfile), "Account");
        }



        [HttpPost]

        public async Task<IActionResult> AddEducation(BuddyProfileEducationPutVM model)
        {
            string UserId = Tools.GetClaimValue(HttpContext, ClaimTypes.NameIdentifier);
            int userBuddyId = await _auth.GetBuddyById(UserId);

            ResponseModel res = await _auth.AddEducation(model, userBuddyId);
            return Ok(res);

        }


        [HttpDelete]
        public async Task<IActionResult> DeleteEducation([FromForm] int EducationId)
        {
            ResponseModel resp = await _auth.DeleteEducation(EducationId);
            return Ok(resp);
        }


        [HttpPost]

        public async Task<IActionResult> AddExperience(BuddyProfileExperiencePutVM model)
        {
            string UserId = Tools.GetClaimValue(HttpContext, ClaimTypes.NameIdentifier);
            int userBuddyId = await _auth.GetBuddyById(UserId);

            ResponseModel res = await _auth.AddExperience(model, userBuddyId);
            return Ok(res);

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteExperience([FromForm] int ExperienceId)
        {
            ResponseModel resp = await _auth.DeleteExperience(ExperienceId);
            return Ok(resp);
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<IActionResult> UserBuddyProfile()
        {
            ResponseModel Buddy = new ResponseModel();

            string UserId = Tools.GetClaimValue(HttpContext, ClaimTypes.NameIdentifier);
            int userBuddyId = await _auth.GetBuddyById(UserId);
            if (userBuddyId != 0)
            {
                Buddy = await _buddy.GetBuddy(userBuddyId, Request);
            }
            return View(Buddy.Data.Data);
        }

        #endregion


        #region Forget Password with email link
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgetPassword([FromForm] string identifier)
        {
            if (!string.IsNullOrEmpty(identifier))
            {
                ResponseModel res = await _auth.ForgetPassword(identifier, Request);
                return Ok(res);
            }
            return Ok(0);
        }

        [HttpPost]
        [AllowAnonymous]

        public async Task<IActionResult> ResetPasswordFromEmail([FromForm] ResetPasswordFromEmail_VM model)
        {
            if (ModelState.IsValid)
            {
                ResponseModel res = await _auth.ResetPasswordFromEmail(model);
                return Ok(res);
            }
            return Ok(0);
        }
        #endregion

        #region Available Toggle
        [HttpPut]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> IsAvailableToggle()
        {
            string UserId = Tools.GetClaimValue(HttpContext, ClaimTypes.NameIdentifier);
            int userBuddyId = await _auth.GetBuddyById(UserId);
            return Ok(await _auth.IsAvailableToggle(userBuddyId));
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
