using HookaTimes.BLL.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;

namespace HookaTimes.API.Controllers
{

    public class SettingsController : APIBaseController
    {
        private readonly ISettingsBL _settingsBL;

        public SettingsController(ISettingsBL settingsBL)
        {
            _settingsBL = settingsBL;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public async Task<IActionResult> GetSettings()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            int userBuddyId = Convert.ToInt32(identity.FindFirst("BuddyID").Value);
            return Ok(await _settingsBL.GetSettings(userBuddyId));
        }
    }
}
