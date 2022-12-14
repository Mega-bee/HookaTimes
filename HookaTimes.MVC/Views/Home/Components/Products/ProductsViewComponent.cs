using HookaTimes.BLL.IServices;
using HookaTimes.BLL.Utilities;
using HookaTimes.BLL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HookaTimes.MVC.Views.Home.Components.Products
{
    public class ProductsViewComponent : ViewComponent
    {
        private readonly IProductBL _productBL;
        private readonly IAuthBO _auth;

        public ProductsViewComponent(IProductBL productBL, IAuthBO auth)
        {
            _productBL = productBL;
            _auth = auth;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string wishlistSessionId = Request.Cookies["WishlistSessionId"]!;
            int BuddyId = 0;
            string UserId = Tools.GetClaimValue(HttpContext, ClaimTypes.NameIdentifier);
            BuddyId = await _auth.GetBuddyById(UserId);



            List<Product_VM> products = await _productBL.GetAllProductsMVC(BuddyId, Request, wishlistSessionId, 6);
            return View(products);
        }
    }
}
