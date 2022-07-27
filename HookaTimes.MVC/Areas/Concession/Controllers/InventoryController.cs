using HookaTimes.BLL.ViewModels.Concession;
using Microsoft.AspNetCore.Mvc;

namespace HookaTimes.MVC.Areas.Concession.Controllers
{
    [Area("Concession")]
    public class InventoryController : Controller
    {
        List<Stock_VM> stock = new List<Stock_VM>();
        List<Orders_VM> orders = new List<Orders_VM>();

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




        private void FillOrderList()
        {
            orders.Add(new Orders_VM()
            {
                OrderID = 718208,
                OrderDate = DateTime.Now.ToString("dddd, dd MMMM yyyy"),
                OrderDescription = "50 Apple Capsule / 30 Grape Cap... ",
                OrderStatus = "Processing"
            });
            orders.Add(new Orders_VM()
            {
                OrderID = 718208,
                OrderDate = DateTime.Now.ToString("dddd, dd MMMM yyyy"),
                OrderDescription = "50 Apple Capsule / 30 Grape Cap... ",
                OrderStatus = "Processing"
            });
            orders.Add(new Orders_VM()
            {
                OrderID = 718208,
                OrderDate = DateTime.Now.ToString("dddd, dd MMMM yyyy"),
                OrderDescription = "50 Apple Capsule / 30 Grape Cap... ",
                OrderStatus = "Processing"
            });
            orders.Add(new Orders_VM()
            {
                OrderID = 718208,
                OrderDate = DateTime.Now.ToString("dddd, dd MMMM yyyy"),
                OrderDescription = "50 Apple Capsule / 30 Grape Cap... ",
                OrderStatus = "Processing"
            });



        }
        #endregion

        #region Inventory

        public IActionResult Stock()
        {
            FillStockList();
            return View("~/Areas/Concession/Views/Pages/Inventory/Stock/Stock.cshtml", stock);
        }
        #endregion

        #region Order

        public IActionResult OrderHistory()
        {
            FillOrderList();
            return View("~/Areas/Concession/Views/Pages/Inventory/OrderHistory/OrderHistory.cshtml", orders);
        }
        #endregion

    }
}
