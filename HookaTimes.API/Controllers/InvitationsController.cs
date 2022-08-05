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

    public class InvitationsController : APIBaseController
    {
        private readonly IInvitationBL _invitationBL;

        public InvitationsController(IInvitationBL invitationBL)
        {
            _invitationBL = invitationBL;
        }

        [HttpPut("{invitationId}")]
        public async Task<IActionResult> SetInvitationStatus([FromRoute] int invitationId,[FromForm] int statusId)
        {
            return Ok(await _invitationBL.SetInvitationStatus(statusId, invitationId));
        }

        [HttpGet]
        public async Task<IActionResult> GetSentInvitations()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            int userBuddyId = Convert.ToInt32(identity.FindFirst("BuddyID").Value);
            return Ok(await _invitationBL.GetSentInvitations(Request,userBuddyId));
        }

        [HttpGet]
        public async Task<IActionResult> GetRecievedInvitations()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            int userBuddyId = Convert.ToInt32(identity.FindFirst("BuddyID").Value);
            return Ok(await _invitationBL.GetRecievedInvitations(Request, userBuddyId));
        }

        [HttpGet]
        public async Task<IActionResult> GetInvitationOptions()
        {
            return Ok(await _invitationBL.GetInvitationOptions());
        }
    }
}
