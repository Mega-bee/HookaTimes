using HookaTimes.BLL.IServices;
using HookaTimes.BLL.Utilities;
using HookaTimes.BLL.ViewModels.Website;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HookaTimes.MVC.Views.Shared.Components.NavBarComp
{
    public class NavBarCompViewComponent : ViewComponent
    {
        private readonly IAuthBO _auth;

        public NavBarCompViewComponent(IAuthBO auth)
        {
            _auth = auth;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string UserId = Tools.GetClaimValue(HttpContext, ClaimTypes.NameIdentifier);
            NavBuddy_VM buddy = await _auth.GetNavBuddyProfile(UserId);
            EmailSignInMVC_VM model = new EmailSignInMVC_VM();

            ViewBag.Buddy = buddy;

            return View(model);
        }
    }
}
