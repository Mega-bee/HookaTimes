using HookaTimes.BLL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HookaTimes.MVC.Areas.Distributor.Controllers
{
    [Area("Distributor")]
    public class PosController : Controller
    {
        public static List<Restaurant_VM> restaurants = new List<Restaurant_VM>();
        public static List<Concession_VM> concessions = new List<Concession_VM>();
        public static List<DeliveryCenter_VM> deliveryCenters = new List<DeliveryCenter_VM>();

        #region Restaurants
        private static void FillRestaurantList()
        {
            restaurants.Add(new Restaurant_VM()
            {
                Id = 1,
                Name = "King of Grill",
                PhoneNumber = "+961 123 456",
                EmailAddress = "badih@kingofgrill.com",
                PersonInCharge = "Badih Abou Hassan"
            }); restaurants.Add(new Restaurant_VM
            {
                Id = 3,
                Name = "Kuazi Cafe",
                PhoneNumber = "+961 123 456",
                EmailAddress = "badih@kuazi.com",
                PersonInCharge = "Badih Abou Hassan",

            });
            restaurants.Add(new Restaurant_VM
            {
                Id = 4,
                Name = "Zanbak Garden Resto",
                PhoneNumber = "+961 123 456",
                EmailAddress = "badih@zanbak.com",
                PersonInCharge = "Badih Abou Hassan",

            });
        }

        public IActionResult Restaurants()
        {
            FillRestaurantList();
            List<RestaurantList_VM> restaurantsList = restaurants.Select(x => new RestaurantList_VM
            {
                Balance = "USD 34,500",
                DateJoined = DateTime.UtcNow.ToShortDateString(),
                Id = x.Id,
                Name = x.Name,
            }).ToList();
            return View("~/Areas/Admin/Views/Pages/Pos/Restaurants/Index.cshtml", restaurantsList);
        }

        public IActionResult Restaurant(int id)
        {
            Restaurant_VM distributor = restaurants.Where(x => x.Id == id).Select(x => new Restaurant_VM
            {
                EmailAddress = x.EmailAddress,
                Id = x.Id,
                Name = x.Name,
                PersonInCharge = x.PersonInCharge,
                PhoneNumber = x.PhoneNumber,
            }).FirstOrDefault()!;
            return View("~/Areas/Admin/Views/Pages/Pos/Restaurants/Restaurant.cshtml", distributor);
        }
        #endregion

        #region Concessions
        private static void FillConcessionList()
        {
            concessions.Add(new Concession_VM()
            {
                Id = 1,
                EmailAddress = "badih@concession.com",
                 Capacity = "20",
                  Dimension = "200x300",
                   Status = "Ready To Go",
                    
                   
            }); concessions.Add(new Concession_VM()
            {
                Id = 1,
                EmailAddress = "hadi@concession.com",
                Capacity = "20",
                Dimension = "200x300",
                Status = "Assigned",


            });
            concessions.Add(new Concession_VM()
            {
                Id = 1,
                EmailAddress = "charbel@concession.com",
                Capacity = "20",
                Dimension = "200x300",
                Status = "Out of Service",


            });
        }

        public IActionResult Concessions()
        {
            FillConcessionList();
            List<ConcessionList_VM> concessionList = concessions.Select(x => new ConcessionList_VM
            {
                DateJoined = DateTime.UtcNow.ToShortDateString(),
                 Status = x.Status,
                Id = x.Id,
            }).ToList();
            return View("~/Areas/Admin/Views/Pages/Pos/Concessions/Index.cshtml", concessionList);
        }

        public IActionResult Concession(int id)
        {
            Concession_VM concession = concessions.Where(x => x.Id == id).FirstOrDefault()!;
            return View("~/Areas/Admin/Views/Pages/Pos/Concessions/Concession.cshtml", concession);
        }
        #endregion

        #region Delivery Center
        private static void FillDeliveryCenterList()
        {
            deliveryCenters.Add(new DeliveryCenter_VM()
            {
                Id = 1,
                EmailAddress = "badih@dwco.com",
                 Name = "Warehouse Dubai A",
                  PersonInCharge = "Badih Abou Hasan",
                   PhoneNumber = "+961 123 456 78",
                    Balance = " USD 24,500"

            }); deliveryCenters.Add(new DeliveryCenter_VM()
            {
                Id = 1,
                EmailAddress = "hadi@dwco.com",
                Name = "HookaTimes Dubai B",
                PersonInCharge = "Hadi Bawarshi",
                PhoneNumber = "+961 123 456 78",
                Balance = " USD 14,500"



            });
            deliveryCenters.Add(new DeliveryCenter_VM()
            {
                Id = 1,
                EmailAddress = "charbel@dwco.com",
                Name = "Warehouse Sharjah B",
                PersonInCharge = "Charbel Mahfouz",
                PhoneNumber = "+961 123 456 78",
                Balance = " USD 240,500"


            });
        }

        public IActionResult DeliveryCenters()
        {
            FillDeliveryCenterList();
            List<DeliveryCenterList_VM> deliveryCenterList = deliveryCenters.Select(x => new DeliveryCenterList_VM
            {
                Name = x.Name,
                 Balance = x.Balance,
                  
                Id = x.Id,
            }).ToList();
            return View("~/Areas/Admin/Views/Pages/Pos/DeliveryCenters/Index.cshtml", deliveryCenterList);
        }

        public IActionResult DeliveryCenter(int id)
        {
            DeliveryCenter_VM deliveryCenter = deliveryCenters.Where(x => x.Id == id).FirstOrDefault()!;
            return View("~/Areas/Admin/Views/Pages/Pos/DeliveryCenters/DeliveryCenter.cshtml", deliveryCenter);
        }
        #endregion
    }
}
