using HookaTimes.BLL.IServices;
using HookaTimes.BLL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HookaTimes.MVC.Controllers
{
    public class InvitationController : Controller
    {
        private readonly IInvitationBL _inv;
        public InvitationController(IInvitationBL inv)
        {
            _inv = inv;
        }

        public async Task<IActionResult> QuickView()
        {
            ResponseModel res = await _inv.GetInvitationOptions();
            ViewBag.options = res.Data.Data;
            return PartialView("/Views/Shared/Ecommerce/_QuickViewPartial.cshtml");
        }
    }
}
