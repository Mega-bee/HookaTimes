using HookaTimes.BLL.IServices;
using HookaTimes.BLL.Utilities;
using HookaTimes.BLL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HookaTimes.API.Controllers
{
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]

    public class CartController : APIBaseController
    {
        private readonly ICartBL _cartBL;

        public CartController(ICartBL cartBL)
        {
            _cartBL = cartBL;
        }

        [HttpPut("{productId}/{quantity}")]
        public async Task<IActionResult> AddToCart([FromRoute] int productId, [FromRoute] int quantity)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            int userBuddyId = Convert.ToInt32(identity.FindFirst("BuddyID").Value);
            return Ok(await _cartBL.AddToCart(userBuddyId, quantity, productId));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCart([FromForm] List<UpdateCartItem_VM> items)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            int userBuddyId = Convert.ToInt32(identity.FindFirst("BuddyID").Value);
            return Ok(await _cartBL.UpdateCart(items, userBuddyId));
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveItemFromCart([FromForm] int productId)
        {

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            int userBuddyId = Convert.ToInt32(identity.FindFirst("BuddyID").Value);
            return Ok(await _cartBL.RemoveItemFromCart(productId, userBuddyId));
        }


        [HttpDelete]
        public async Task<IActionResult> ClearCart()
        {

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            int userBuddyId = Convert.ToInt32(identity.FindFirst("BuddyID").Value);
            return Ok(await _cartBL.ClearCart(userBuddyId));
        }

        [HttpGet]
        public async Task<IActionResult> GetCartSummary()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            int userBuddyId = Convert.ToInt32(identity.FindFirst("BuddyID").Value);
            return Ok(await _cartBL.GetCartSummary(Request, userBuddyId));
        }
    }
}
