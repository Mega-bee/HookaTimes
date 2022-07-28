﻿using HookaTimes.BLL.ViewModels.Restaurant;
using Microsoft.AspNetCore.Mvc;

namespace HookaTimes.MVC.Areas.Restaurant.Controllers
{
    [Area("Restaurant")]
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
                Description = "Product description",
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
            return View("~/Areas/Restaurant/Views/Pages/Products/Index.cshtml", productsList);
        }
    }
}
