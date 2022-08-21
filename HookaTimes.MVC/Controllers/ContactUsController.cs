using HookaTimes.BLL.IServices;
using HookaTimes.BLL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HookaTimes.MVC.Controllers
{
    public class ContactUsController : Controller
    {
        private readonly IContactUsBL _contactUsBL;

        public ContactUsController(IContactUsBL contactUsBL)
        {
            _contactUsBL = contactUsBL;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SendContactUsMessage(ContactUs_VM model)
        {
            ResponseModel resp = await _contactUsBL.SendContactUsMessage(model);
            return Ok(resp);
        }
    }
}
