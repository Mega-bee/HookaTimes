using HookaTimes.BLL.IServices;
using HookaTimes.BLL.ViewModels.Website;
using HookaTimes.DAL.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        #region MyRegion

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

            ClaimsIdentity identity = await _auth.SignUpWithEmailMVC(model);
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

            return RedirectToAction("Index", "Home");


            //if (ModelState.IsValid)
            //{

            //    return LocalRedirect(returnurl);

            //}
            //return View(model);
        }
        #endregion


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(EmailSignInMVC_VM model, string returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            returnurl = returnurl ?? Url.Content("~/");

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



        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
            //return RedirectToAction(nameof(HomeController.Index), nameof(HomeController));
        }
    }
}
