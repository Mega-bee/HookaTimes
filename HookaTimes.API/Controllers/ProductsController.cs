using HookaTimes.BLL.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HookaTimes.API.Controllers
{
    //[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]

    public class ProductsController : APIBaseController
    {
        private readonly IProductBL _products;

        public ProductsController(IProductBL products)
        {
            _products = products;
        }

        #region Categories
        [HttpGet]

        public async Task<IActionResult> GetAllCategories()
        {
            return Ok(await _products.GetAllCategories(Request));
        }
        #endregion


        #region Products
        [HttpGet("{id}")]

        public async Task<IActionResult> GetCategoryProducts([FromRoute] int id)
        {
            return Ok(await _products.GetProductsByCategoryId(id, Request));
        }
        #endregion
    }
}
