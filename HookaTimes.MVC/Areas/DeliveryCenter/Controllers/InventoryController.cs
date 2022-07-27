using HookaTimes.BLL.ViewModels.DeliveryCenter;
using Microsoft.AspNetCore.Mvc;

namespace HookaTimes.MVC.Areas.DeliveryCenter.Controllers
{
    [Area("DeliveryCenter")]
    public class InventoryController : Controller
    {
        public static List<Stock_VM> stocks = new List<Stock_VM>();
        public static List<Order_VM> orders = new List<Order_VM>();
        #region Lists
        private void FillStocks()
        {
            stocks.Add(new Stock_VM()
            {
                Id = 1,
                Name = "Lemon & Mint",
                Category = "Capsule",
                Quantity= 2023,
                Status="In Stock"

            }); stocks.Add(new Stock_VM
            {
                Id = 2,
                Name = "Apple",
                Category = "Capsule",
                Quantity=1700,
                Status= "In Stock"

            });
            stocks.Add(new Stock_VM
            {
                Id = 3,
                Name = "Liquorice",
                Category = "Capsule",
                Quantity=3000,
                Status="In Stock"

            });
            stocks.Add(new Stock_VM
            {
                Id = 4,
                Name = "Hooka Witty",
                Category = "Hooka",
                Quantity=123.040,
                Status= "In Stock"

            });
            stocks.Add(new Stock_VM
            {
                Id = 5,
                Name = "Coals",
                Category = "Accessories",
                Quantity = 3500,
                Status= "In Stock"

            });
            stocks.Add(new Stock_VM
            {
                Id = 6,
                Name = "Plastic Hose",
                Category = "Accessories",
                Quantity = 12400,
                Status= "In Stock"

            });
        }
        #endregion

        #region Order List
        public void FillOrders()
        {
            orders.Add(new Order_VM()
            {
                Id = 1,
                CreatedDate="27 June 2022",
                Description = "4500 Apple Capsule / 3000 Grape Capsule",
                Status = "Cancelled"

            }); orders.Add(new Order_VM
            {
                Id = 2,
                CreatedDate = "26 June 2022",
                Description = "1500 Lemon Capsule / 3500 Coals",
                Status = "Cancelled"

            });
            orders.Add(new Order_VM
            {
                Id = 3,
                CreatedDate = "25 June 2022",
                Description = "5500 Apple Capsule / 3600 Grape Capsule",
                Status = "Cancelled"

            });
            orders.Add(new Order_VM
            {
                Id = 4,
                CreatedDate = "24 June 2022",
                Description = "4000 Lemon Capsule / 3000 Coals",
                Status = "Cancelled"

            });
            orders.Add(new Order_VM
            {
                Id = 5,
                CreatedDate = "24 June 2022",
                Description = "5000 Apple Capsule / 3000 Grape Capsule",
                Status = "Cancelled"

            });
            orders.Add(new Order_VM
            {
                Id = 6,
                CreatedDate = "23 June 2022",
                Description = "4000 Lemon Capsule / 3000 Coals",
                Status = "Cancelled"

            });
        }

        #endregion

        public IActionResult Stocks()
        {
            FillStocks();
            List<StocksList_VM> stocksList = stocks.Select(x => new StocksList_VM
            {
                Id = x.Id,
                Name = x.Name,
                Category = x.Category,
                Quantity = x.Quantity,
                Status = x.Status

            }).ToList();
            return View("~/Areas/DeliveryCenter/Views/Pages/Inventory/Stocks.cshtml", stocksList);
        }

        public IActionResult Orders()
        {
            FillOrders();
            List<OrdersList_VM> ordersList = orders.Select(x => new OrdersList_VM
            {
                Id = x.Id,
                CreatedDate = x.CreatedDate,
                Description = x.Description,
                Status = x.Status
            }).ToList();
            return View("~/Areas/DeliveryCenter/Views/Pages/Inventory/Orders.cshtml", ordersList);
        }

        public IActionResult CreateOrder()
        {
            return View("~/Areas/DeliveryCenter/Views/Pages/Inventory/CreateOrder.cshtml");
        }

    }
}

