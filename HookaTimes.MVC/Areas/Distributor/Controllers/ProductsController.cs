using HookaTimes.BLL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HookaTimes.MVC.Areas.Distributor.Controllers
{
    [Area("Distributor")]
    public class ProductsController : Controller
    {
        public static List<Product_VM> products = new List<Product_VM>();
        #region Lists
        private void FillList()
        {
            products.Add(new Product_VM()
            {
                Id = 1,
                Name = "Lemon & Mint",
                 Category = "Capsule",
                  Description ="Product description",
                   UnitPrice = "50/lot",
            }); products.Add(new Product_VM
            {
                Id = 2,
                Name = "Apple",
                Category = "Capsule",
                Description = "Product description",
                UnitPrice = "50/lot",

            });
            products.Add(new Product_VM
            {
                Id = 3,
                Name = "Liquorice",
                Category = "Capsule",
                Description = "Product description",
                UnitPrice = "50/lot",

            });
            products.Add(new Product_VM
            {
                Id = 4,
                Name = "Hooka Witty",
                Category = "Hooka",
                Description = "Product description",
                UnitPrice = "20/lot",

            });
        }
        #endregion
        public IActionResult Index()
        {
            FillList();
            List<ProductsList_VM> productsList = products.Select(x => new ProductsList_VM
            {
                UnitPrice = x.UnitPrice,
                Category = x.Category,
                Id = x.Id,
                Name = x.Name,
            }).ToList();
            return View("~/Areas/Distributor/Views/Pages/Products/Index.cshtml",productsList);
        }

        public IActionResult Product(int id)
        {
            Product_VM product = products.Where(x=> x.Id == id).Select(x => new Product_VM
            {
                UnitPrice = x.UnitPrice,
                Category = x.Category,
                Id = x.Id,
                Name = x.Name,
            }).FirstOrDefault()!;
            return View("~/Areas/Distributor/Views/Pages/Products/Product.cshtml", product);
        }

        public IActionResult CreateProduct(int id)
        {
            return View("~/Areas/Distributor/Views/Pages/Products/CreateProduct.cshtml");
        }


    }
}
