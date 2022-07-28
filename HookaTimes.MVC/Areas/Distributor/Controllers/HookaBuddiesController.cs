using HookaTimes.BLL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HookaTimes.MVC.Areas.Distributor.Controllers
{
    [Area("Distributor")]
    public class HookaBuddiesController : Controller
    {

        public static List<HookaBuddy_VM> hookaBuddies = new List<HookaBuddy_VM>();

        #region Lists
        private void FillList()
        {
            hookaBuddies.Add(new HookaBuddy_VM()
            {
                Id = 1,
                Name = "Hasan Bdeir",
                PhoneNumber = "+961 123 456",
                EmailAddress = "h.bdeir@hotmail.com",
                 Address = "Beirut",
                  Image = "300-1.jpg",
                 DateOfBirth = DateTime.UtcNow.ToString(),
            }); hookaBuddies.Add(new HookaBuddy_VM
            {
                Id = 2,
                Name = "Hadi Bawarshi",
                PhoneNumber = "+961 123 456",
                EmailAddress = "hadi.bawarshi@gmail.com",
                Address = "Beirut",
                Image = "300-1.jpg",
                DateOfBirth = DateTime.UtcNow.ToString(),

            });
        }
        #endregion

        public IActionResult Index()
        {
            FillList();
            List<HookaBuddiesList_VM> buddiesList = hookaBuddies.Select(x => new HookaBuddiesList_VM
            {
                Id = x.Id,
                Image = x.Image,
                Name = x.Name,

            }).ToList();
            return View("~/Areas/Distributor/Views/Pages/HookaBuddies/Index.cshtml", buddiesList);
        }

        public IActionResult HookaBuddy(int id)
        {
            HookaBuddy_VM buddy = hookaBuddies.Where(x=> x.Id == id).Select(x => new HookaBuddy_VM
            {
                Id = x.Id,
                Image = x.Image,
                Name = x.Name,
                Address = x.Address,
                DateOfBirth = x.DateOfBirth,
                EmailAddress = x.EmailAddress,
                PhoneNumber = x.PhoneNumber,

            }).FirstOrDefault()!;

            return View("~/Areas/Distributor/Views/Pages/HookaBuddies/HookaBuddy.cshtml", buddy);
        }
    }
}
