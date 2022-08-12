using HookaTimes.BLL.IServices;
using HookaTimes.BLL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CharbelFrennPortolfio.Views.Home.Components.Contact
{
    public class ProductsViewComponent : ViewComponent
    {
        private readonly IProductBL _productBL;

        public ProductsViewComponent(IProductBL productBL)
        {
            _productBL = productBL;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            int userBuddyId = 0;
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity!.IsAuthenticated)
            {
                userBuddyId = Convert.ToInt32(identity.FindFirst("BuddyID")!.Value);
            }
            List<Product_VM> products = await _productBL.GetAllProductsMVC(userBuddyId, Request);
            return View(products);
        }
    }
}
