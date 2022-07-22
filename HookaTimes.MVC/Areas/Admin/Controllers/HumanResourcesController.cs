using HookaTimes.BLL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HookaTimes.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HumanResourcesController : Controller
    {
        public static List<ViewUser_VM> users = new List<ViewUser_VM>();
        private void FillList()
        {
            users.Add(new ViewUser_VM() 
            {
                Id = 1,
                Name = "David Mechelany",
                  DateOfBirth = DateTime.Now,
                   Email = "dave@gmail.com",
                    Password ="P@ssw0rd",
                     Role = "User",
                      RoleId = Guid.NewGuid().ToString(),

            }); users.Add(new ViewUser_VM
            {
                Id = 2,
                Name = "Hadi Bawarshi",
                DateOfBirth = DateTime.Now,
                Email = "hadibawarshi@gmail.com",
                Password = "P@ssw0rd",
                Role = "Admin",
                RoleId = Guid.NewGuid().ToString(),

            }); users.Add(new ViewUser_VM
            {
                Id = 3,
                Name = "Charbel Mahfouz",
                DateOfBirth = DateTime.Now,
                Email = "charbel@gmail.com",
                Password = "P@ssw0rd",
                Role = "User",
                RoleId = Guid.NewGuid().ToString(),

            }); users.Add(new ViewUser_VM
            {
                Id = 4,
                Name = "Kamal Frenn",
                DateOfBirth = DateTime.Now,
                Email = "kamalfrenn@gmail.com",
                Password = "P@ssw0rd",
                Role = "Admin",
                RoleId = Guid.NewGuid().ToString(),

            });
        }

        public IActionResult HookaTimesStaff()
        {
            FillList();
            List<UserList_VM> userlist = users.Select(x=> new UserList_VM
            {
                 Id=x.Id,
                  Name=x.Name,
            }).ToList();
            return View("~/Areas/Admin/Views/Pages/HumanResources/HookaTimesStaff.cshtml", userlist);
        }

        public IActionResult VuewUser(int id)
        {
            ViewUser_VM user = users.Where(x=> x.Id == id).FirstOrDefault()!; 
            return View("~/Areas/Admin/Views/Pages/HumanResources/HookaTimesStaff.cshtml", user);
        }
    }
}
