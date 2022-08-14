using HookaTimes.BLL.IServices;
using HookaTimes.BLL.Service;
using HookaTimes.BLL.ViewModels;
using HookaTimes.BLL.ViewModels.Website;
using Microsoft.AspNetCore.Mvc;

namespace HookaTimes.MVC.Views.Home.Components.BuddiesSearchResult
{
    public class BuddiesSearchResult : ViewComponent
    {
        private readonly IHookaBuddyBL _hookaBuddyBL;

        public BuddiesSearchResult(IHookaBuddyBL hookaBuddyBL)
        {
            _hookaBuddyBL = hookaBuddyBL;
        }

        public async Task<IViewComponentResult> InvokeAsync(int userBuddyId, int filterBy= 0, int sortBy = 0)
        {
            try
            {
                List<Buddy_VM> buddies = await _hookaBuddyBL.GetBuddiesMVC(Request, userBuddyId, 0,filterBy,sortBy);
                return View(buddies);
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}
