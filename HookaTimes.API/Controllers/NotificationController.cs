using HookaTimes.BLL.IServices;
using HookaTimes.BLL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HookaTimes.API.Controllers
{
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
    public class NotificationController : APIBaseController
    {
        private readonly IHookaNotificationBL _hookaNotificationBL;

        public NotificationController(IHookaNotificationBL hookaNotificationBL)
        {
            _hookaNotificationBL = hookaNotificationBL;
        }

        [HttpGet]
        public async Task<IActionResult> SentNot()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            int userBuddyId = Convert.ToInt32(identity.FindFirst("BuddyID").Value);

            return Ok(await _hookaNotificationBL.GetSentNotification(Request, userBuddyId));
        }


    }
}
