using HookaTimes.BLL.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HookaTimes.API.Controllers
{
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]

    public class OrdersController : APIBaseController
    {
        private readonly IOrderBL _orderBL;

        public OrdersController(IOrderBL orderBL)
        {
            _orderBL = orderBL;
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder([FromForm] int addressId)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            int userBuddyId = Convert.ToInt32(identity.FindFirst("BuddyID").Value);
            return Ok(await _orderBL.PlaceOrder(userBuddyId, addressId));
        }
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            int userBuddyId = Convert.ToInt32(identity.FindFirst("BuddyID").Value);
            return Ok(await _orderBL.GetOrders(userBuddyId));
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrder([FromRoute] int orderId)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            int userBuddyId = Convert.ToInt32(identity.FindFirst("BuddyID").Value);
            return Ok(await _orderBL.GetOrder(Request,userBuddyId, orderId));
        }

    }
}
