using HookaTimes.BLL.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CharbelFrennPortolfio.Views.Home.Components.Services
{
    public class BuddiesViewComponent : ViewComponent
    {
        private readonly IHookaBuddyBL _hookaBuddyBl;

        public BuddiesViewComponent(IHookaBuddyBL hookaBuddyBl)
        {
            _hookaBuddyBl = hookaBuddyBl;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            int userBuddyId = 0;
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity!.IsAuthenticated)
            {
                 userBuddyId = Convert.ToInt32(identity.FindFirst("BuddyID")!.Value);
            }
            var items = await _hookaBuddyBl.GetBuddiesMVC(Request,userBuddyId,6);
            return View(items);
        }
    }
}
