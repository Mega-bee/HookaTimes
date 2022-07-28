using HookaTimes.BLL.ViewModels.Concession;
using Microsoft.AspNetCore.Mvc;

namespace HookaTimes.MVC.Areas.Concession.Controllers
{
    public class InventoriesController : Controller
    {
        List<Stock_VM> stock = new List<Stock_VM>();

        #region ListFill
        private void FillStockList()
        {
            stock.Add(new Stock_VM()
            {
                Product = "Lemon&Mint",
                Category = "Capsule",
                Quantity = 23,
                Status = "Low Qty"
            });
            stock.Add(new Stock_VM()
            {
                Product = "Lemon&Mint",
                Category = "Capsule",
                Quantity = 23,
                Status = "Low Qty"
            });
            stock.Add(new Stock_VM()
            {
                Product = "Lemon&Mint",
                Category = "Capsule",
                Quantity = 23,
                Status = "Low Qty"
            });
            stock.Add(new Stock_VM()
            {
                Product = "Lemon&Mint",
                Category = "Capsule",
                Quantity = 23,
                Status = "Low Qty"
            });

        }
        #endregion

        #region Inventory

        public IActionResult Stock()
        {
            FillStockList();
            return View("~/Areas/Concession/Views/Pages/Inventory/Stock/Stock.cshtml");
        }

        #endregion
    }
}
