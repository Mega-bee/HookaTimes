using HookaTimes.BLL.IServices;
using HookaTimes.BLL.Utilities;
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


        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
          RoleManager<IdentityRole> roleManager, IAuthBO auth)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _auth = auth;
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
                IdentityResult res = await _auth.SignUpWithEmailMVC(model);



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


            ClaimsIdentity identity = await _auth.EmailSignInMVC(model);
            if (identity == null)
            {
                return View(model);

            }
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            //Set current principal
            Thread.CurrentPrincipal = principal;

            User.AddIdentity(identity);


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
            if (ModelState.IsValid)
            {
                string UID = Tools.GetClaimValue(HttpContext, ClaimTypes.NameIdentifier);

                var user = await _userManager.FindByIdAsync(UID);

                if (user == null)
                    return View(model);

                if (!await _userManager.CheckPasswordAsync(user, model.CurrentPassword))
                    return View(model);
                if (model.NewPassword == model.CurrentPassword)
                    return View(model);
                if (model.NewPassword != model.ConfirmPassword)
                    return View(model);
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var encodedToken = Encoding.UTF8.GetBytes(token);
                var validToken = WebEncoders.Base64UrlEncode(encodedToken);
                var decodedToken = WebEncoders.Base64UrlDecode(validToken);
                string normalToken = Encoding.UTF8.GetString(decodedToken);

                var result = await _userManager.ResetPasswordAsync(user, normalToken, model.NewPassword);
                if (result.Succeeded)
                    return Ok(new { message = "Password has been reset succesfully" });

                return View(model);
            }
            return View(model);
        }

        #endregion





    }
}
