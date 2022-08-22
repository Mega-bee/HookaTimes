using HookaTimes.BLL.IServices;
using HookaTimes.BLL.ViewModels.Website;
using Microsoft.AspNetCore.Mvc;

namespace HookaTimes.MVC.Controllers
{
    public class BecomeAPartnerController : Controller
    {
        private readonly IBecomeAPartnerBL _becomeAPartnerBL;

        public BecomeAPartnerController(IBecomeAPartnerBL becomeAPartnerBL)
        {
            _becomeAPartnerBL = becomeAPartnerBL;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> BecomeAPartner(BecomeAPartner_VM model)
        {
            return Ok(await _becomeAPartnerBL.SendPartnerRequest(model));
        }
    }
}
