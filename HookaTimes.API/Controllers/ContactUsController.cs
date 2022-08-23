using HookaTimes.BLL.IServices;
using HookaTimes.BLL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HookaTimes.API.Controllers
{
    public class ContactUsController : APIBaseController
    {
        private readonly IContactUsBL _contactUsBL;

        public ContactUsController(IContactUsBL contactUsBL)
        {
            _contactUsBL = contactUsBL;
        }

        [HttpPost]
        public async Task<IActionResult> SendContactUsMessage([FromForm]ContactUs_VM model)
        {
            ResponseModel resp = await _contactUsBL.SendContactUsMessage(model);
            return Ok(resp);
        }
    }
}
