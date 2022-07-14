using Microsoft.AspNetCore.Mvc;

namespace HookaTimes.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Areas/Admin/Views/Pages/Products/Index.cshtml");
        }

        public IActionResult Edit()
        {
            return View("~/Areas/Admin/Views/Pages/Products/EditProduct.cshtml");
        }

        public IActionResult Add()
        {
            return View("~/Areas/Admin/Views/Pages/Products/AddProduct.cshtml");
        }
    }
}
