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
            int BuddyId = 0;
            string UserId = Tools.GetClaimValue(HttpContext, ClaimTypes.NameIdentifier);
            var buddy = await _auth.GetBuddyById(UserId);
            if (buddy != null)
            {
                BuddyId = buddy.Id;
            }


            List<Product_VM> products = await _productBL.GetAllProductsMVC(BuddyId, Request, 6);
            return View(products);
        }
    }
}
