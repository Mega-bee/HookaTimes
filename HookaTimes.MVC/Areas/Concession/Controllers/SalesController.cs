using HookaTimes.BLL.ViewModels.Concession;
using Microsoft.AspNetCore.Mvc;

namespace HookaTimes.MVC.Areas.Concession.Controllers
{

    [Area("Concession")]
    public class SalesController : Controller
    {
        List<SalesList_VM> SalesList = new List<SalesList_VM>();

        #region ListFill
        private void FillSalesList()
        {
            SalesList.Add(new SalesList_VM()
            {
                OrderId = 1,
                OrderDate = DateTime.Now.ToString("dddd, dd MMMM yyyy"),
                CustomerName = "Hadi Bawarshi",
                CustomerOrder = "2x Apple Hooka Witty",
                Total = "USD 30",
            });
            SalesList.Add(new SalesList_VM()
            {
                OrderId = 2,
                OrderDate = DateTime.Now.ToString("dddd, dd MMMM yyyy"),
                CustomerName = "Hadi Bawarshi",
                CustomerOrder = "2x Apple Hooka Witty",
                Total = "USD 30",
            });

            SalesList.Add(new SalesList_VM()
            {
                OrderId = 3,
                OrderDate = DateTime.Now.ToString("dddd, dd MMMM yyyy"),
                CustomerName = "Hadi Bawarshi",
                CustomerOrder = "2x Apple Hooka Witty",
                Total = "USD 30",
            });

            SalesList.Add(new SalesList_VM()
            {
                OrderId = 4,
                OrderDate = DateTime.Now.ToString("dddd, dd MMMM yyyy"),
                CustomerName = "Hadi Bawarshi",
                CustomerOrder = "2x Apple Hooka Witty",
                Total = "USD 30",
            });
        }
        #endregion

        #region Indoor;                
        public IActionResult Indoor()
        {
            FillSalesList();

            return View("~/Areas/Concession/Views/Pages/Sales/Indoor/Index.cshtml", SalesList);
        }

        public IActionResult Pos()
        {
            FillSalesList();

            return View("~/Areas/Concession/Views/Pages/Sales/Indoor/Pos.cshtml");
        }


        public IActionResult Outdoor()
        {
            FillSalesList();

            return View("~/Areas/Concession/Views/Pages/Sales/Outdoor/Outdoor.cshtml", SalesList);
        }


        #endregion

    }
}
