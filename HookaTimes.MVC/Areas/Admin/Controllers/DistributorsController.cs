using HookaTimes.BLL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HookaTimes.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DistributorsController : Controller
    {
        public static List<Distributor_VM> distributors = new List<Distributor_VM>();

        private void FillList()
        {
            distributors.Add(new Distributor_VM()
            {
                Id = 1,
                Name = "ShishPresso Distribution",
                PhoneNumber = "+961 123 456",
                 EmailAddress = "badih@shishapresso.com",
                  PersonInCharge = "Badih Abou Hassan"
            }); distributors.Add(new Distributor_VM
            {
                Id = 2,
                Name = "Kuazi Distribution",
                PhoneNumber = "+961 123 456",
                 EmailAddress="badih@kuazi.com",
                  PersonInCharge = "Badih Abou Hassan",
                   
            }); 
        }
        public IActionResult Index()
        {
            FillList();
            List<DistributorList_VM> distributorsList = distributors.Select(x => new DistributorList_VM
            {
                Balance = "USD 34,500",
                DateJoined = DateTime.UtcNow,
                Id = x.Id,
                Name = x.Name,
            }).ToList();
            return View("~/Areas/Admin/Views/Pages/Distributors/Index.cshtml",distributorsList);
        }

        public IActionResult Distributor(int id)
        {
            FillList();
        Distributor_VM distributor = distributors.Where(x => x.Id == id).Select(x=> new Distributor_VM
        {
             CompanyName = x.CompanyName,
              EmailAddress = x.EmailAddress,
               Id = x.Id,
                Name=x.Name,
                 PersonInCharge=x.PersonInCharge,
                  PhoneNumber = x.PhoneNumber,
        }).FirstOrDefault()!;
            return View("~/Areas/Admin/Views/Pages/Distributors/Distributor.cshtml", distributor);
        }
    }
}
