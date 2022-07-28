using HookaTimes.BLL.ViewModels.Restaurant;
using Microsoft.AspNetCore.Mvc;

namespace HookaTimes.MVC.Areas.Restaurant.Controllers
{
    [Area("Restaurant")]
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
                Description = "2x Apple Hooka Witty / 2x Chicken Sandwich",
                Total = "USD30"
            });
            sales.Add(new Sale_VM
            {
                Id = 718207,
                CreatedDate = "26 June 2022",
                Description = "3x Lemon&Mint Hooka Witty / 2x Shawarma Plate",
                Total = "USD45"
            });
            sales.Add(new Sale_VM
            {
                Id = 718206,
                CreatedDate = "25 June 2022",
                Description = "2x Liquorice Hooka Witty / Salmon Salad",
                Total = "USD30"
            });
            sales.Add(new Sale_VM
            {
                Id = 718205,
                CreatedDate = "24 June 2022",
                Description = "2x Apple Hooka Witty / 2x Chicken Sandwich",
                Total = "USD30"
            });
            sales.Add(new Sale_VM
            {
                Id = 718204,
                CreatedDate = "24 June 2022",
                Description = "3x Lemon&Mint Hooka Witty / 2x Orange Juice",
                Total = "USD45"
            });
            sales.Add(new Sale_VM
            {
                Id = 718203,
                CreatedDate = "23 June 2022",
                Description = "1x Lemon&Mint Hooka Witty ",
                Total = "USD15"
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
                Description = x.Description,
                Total = x.Total
            }).ToList();

            return View("~/Areas/Restaurant/Views/Pages/Sales/Index.cshtml", salesList);
        }

        //public IActionResult CreateSale()
        //{
        //    return View("~/Areas/DeliveryCenter/Views/Pages/Sales/CreateSale.cshtml");
        //}

    }
}
