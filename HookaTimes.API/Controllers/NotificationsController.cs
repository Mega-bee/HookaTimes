using HookaTimes.BLL.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System;
using System.Threading.Tasks;

namespace HookaTimes.API.Controllers
{
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]

    public class NotificationsController : APIBaseController
    {
        private readonly INotificationBL _notificationBL;

        public NotificationsController(INotificationBL notificationBL)
        {
            _notificationBL = notificationBL;
        }

        [HttpGet]
        public async Task<IActionResult> GetNotifications()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            int userBuddyId = Convert.ToInt32(identity.FindFirst("BuddyID").Value);
            return Ok(await _notificationBL.GetNotifications(userBuddyId));
        }
    }
}
