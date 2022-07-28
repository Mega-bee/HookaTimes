using HookaTimes.BLL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HookaTimes.MVC.Areas.Distributor.Controllers
{
    [Area("Distributor")]
    public class HookaTeamController : Controller
    {
        public static List<HookaTimesTeamMember_VM> team = new List<HookaTimesTeamMember_VM>();

        #region Lists
        private void FillTeamList()
        {
            team.Add(new HookaTimesTeamMember_VM()
            {
                Id = 1,
                Name = "David Mechelany",
                DateOfBirth = DateTime.Now,
                Email = "dave@gmail.com",
                PhoneNumber = "+961 123 456",
                Address = "Zahle, Lebanon",
                Image = "~/AdminAssets/assets/media/avatars/300-1.jpg",
                ConcessionId = 1,
                Location = "Dubai"
            }); team.Add(new HookaTimesTeamMember_VM
            {
                Id = 2,
                Name = "Hadi Bawarshi",
                DateOfBirth = DateTime.Now,
                Email = "hadibawarshi@gmail.com",
                PhoneNumber = "+961 123 456",
                Address = "Zahle, Lebanon",
                Image = "~/AdminAssets/assets/media/avatars/300-1.jpg",
                ConcessionId = 1,
                Location = "Dubai"
            }); team.Add(new HookaTimesTeamMember_VM
            {
                Id = 3,
                Name = "Charbel Mahfouz",
                DateOfBirth = DateTime.Now,
                Email = "charbel@gmail.com",
                PhoneNumber = "+961 123 456",
                Address = "Zahle, Lebanon",
                Image = "~/AdminAssets/assets/media/avatars/300-1.jpg",
                ConcessionId = 1,
                Location = "Dubai"
            }); team.Add(new HookaTimesTeamMember_VM
            {
                Id = 4,
                Name = "Kamal Frenn",
                DateOfBirth = DateTime.Now,
                Email = "kamalfrenn@gmail.com",
                PhoneNumber = "+961 123 456",
                Address = "Zahle, Lebanon",
                Image = "~/AdminAssets/assets/media/avatars/300-1.jpg",
                ConcessionId = 1,
                Location = "Dubai"
            });
        }
        #endregion

        public IActionResult Index()
        {
            FillTeamList();
            List<HookaTimesTeamList_VM> teamList = team.Select(x => new HookaTimesTeamList_VM
            {
                Id = x.Id,
                Image = x.Image,
                Name = x.Name,
                
            }).ToList();
            return View("~/Areas/Distributor/Views/Pages/HookaTeam/HookaTimesTeam.cshtml", teamList);
        }

        public IActionResult HookaTeamMemberDetails(int id)
        {
            HookaTimesTeamMember_VM user = team.Where(x => x.Id == id).FirstOrDefault()!;
            return View("~/Areas/Distributor/Views/Pages/HookaTeam/HookaTeamMemberDetails.cshtml", user);
        }
    }
}
