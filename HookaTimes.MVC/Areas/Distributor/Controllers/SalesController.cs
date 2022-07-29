using HookaTimes.BLL.ViewModels.DeliveryCenter;
using Microsoft.AspNetCore.Mvc;

namespace HookaTimes.MVC.Areas.Distributor.Controllers
{
    [Area("Distributor")]
    public class SalesController : Controller
    {
        public static List<Sale_VM> sales = new List<Sale_VM>();

        #region Sales List
        private void FillSales()
        {
            sales.Add(new Sale_VM
            {
                Id = 718208,
                CreatedDate = "27 June 2022",
                POSName = "King of Grill",
                Description = "50 Apple Capsule / 30 Grape Capsule",
                Status = "Processing"
            });
            sales.Add(new Sale_VM
            {
                Id = 718207,
                CreatedDate = "26 June 2022",
                POSName = "Noodle Doodle",
                Description = "40 Lemon Capsule / 35 Coals",
                Status = "Processing"
            });
            sales.Add(new Sale_VM
            {
                Id = 718206,
                CreatedDate = "25 June 2022",
                POSName = "King of Grill",
                Description = "55 Apple Capsule / 36 Grape Capsule",
                Status = "Processing"
            });
            sales.Add(new Sale_VM
            {
                Id = 718205,
                CreatedDate = "24 June 2022",
                POSName = "Vlcenzlo Resto",
                Description = "40 Lemon Capsule / 30 Coals",
                Status = "Processing"
            });
            sales.Add(new Sale_VM
            {
                Id = 718204,
                CreatedDate = "24 June 2022",
                POSName = "Kuazl Cafe",
                Description = "50 Apple Capsule / 30 Grape Capsule",
                Status = "Processing"
            });
            sales.Add(new Sale_VM
            {
                Id = 718203,
                CreatedDate = "23 June 2022",
                POSName = "Zanbak Resto",
                Description = "40 Lemon Capsule / 30 Coals",
                Status = "Processing"
            });
        }
        #endregion
        public IActionResult Index()
        {
            FillSales();
            List<SalesList_VM> salesList = sales.Select(x => new SalesList_VM
            {
                Id = x.Id,
                CreatedDate = x.CreatedDate,
                POSName = x.POSName,
                Description = x.Description,
                Status = x.Status
            }).ToList();
            return View("~/Areas/Distributor/Views/Pages/Sales/Index.cshtml", salesList);
        }
        public IActionResult CreateSale()
        {
            return View("~/Areas/Distributor/Views/Pages/Sales/CreateSale.cshtml");
        }
    }
}
