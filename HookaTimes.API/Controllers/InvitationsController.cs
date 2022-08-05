using HookaTimes.BLL.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HookaTimes.API.Controllers
{
  
    public class InvitationsController : APIBaseController
    {
        private readonly IInvitationBL _invitationBL;
        public async Task<IActionResult> SetInvitationStatus([FromRoute] int invitationId,[FromForm] int statusId)
        {
            return Ok(await _invitationBL.SetInvitationStatus(statusId, invitationId));
        }
    }
}
