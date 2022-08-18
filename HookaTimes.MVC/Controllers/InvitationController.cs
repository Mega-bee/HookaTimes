using HookaTimes.BLL.IServices;
using HookaTimes.BLL.ViewModels;
using HookaTimes.BLL.ViewModels.Website;
using Microsoft.AspNetCore.Mvc;

namespace HookaTimes.MVC.Controllers
{
    public class InvitationController : Controller
    {
        private readonly IInvitationBL _inv;
        private readonly IHookaPlaceBL _pl;
        public InvitationController(IInvitationBL inv, IHookaPlaceBL pl)
        {
            _inv = inv;
            _pl = pl;
        }

        public async Task<IActionResult> QuickView()
        {
            ResponseModel res = await _inv.GetInvitationOptions();
            ViewBag.options = res.Data.Data;
            List<PlacesNames_VM> pl = await _pl.GetPlacesNames();
            ViewBag.places = pl;

            return PartialView("/Views/Shared/Ecommerce/_QuickViewPartial.cshtml");
        }
    }
}
