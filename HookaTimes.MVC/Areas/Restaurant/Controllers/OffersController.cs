using HookaTimes.BLL.ViewModels.Restaurant;
using Microsoft.AspNetCore.Mvc;

namespace HookaTimes.MVC.Areas.Restaurant.Controllers
{
    [Area("Restaurant")]
    public class OffersController : Controller
    {

        List<Offers_VM> offers = new List<Offers_VM>();

        #region ListFill
        private void FillOffersList()
        {
            offers.Add(new Offers_VM()
            {
                OfferID = 718208,
                OfferDate = DateTime.Now.ToString("dddd, dd MMMM yyyy"),
                OfferDescription = "30% discount on 2nd order",
                OfferStatus = "Active"
            });

            offers.Add(new Offers_VM()
            {
                OfferID = 718208,
                OfferDate = DateTime.Now.ToString("dddd, dd MMMM yyyy"),
                OfferDescription = "30% discount on 2nd order",
                OfferStatus = "Active"
            });

            offers.Add(new Offers_VM()
            {
                OfferID = 718208,
                OfferDate = DateTime.Now.ToString("dddd, dd MMMM yyyy"),
                OfferDescription = "30% discount on 2nd order",
                OfferStatus = "Active"
            });

            offers.Add(new Offers_VM()
            {
                OfferID = 718208,
                OfferDate = DateTime.Now.ToString("dddd, dd MMMM yyyy"),
                OfferDescription = "30% discount on 2nd order",
                OfferStatus = "Active"
            });


        }
        #endregion
        public IActionResult OffersList()
        {
            FillOffersList();
            return View("~/Areas/Restaurant/Views/Pages/Offers/Offers.cshtml", offers);

        }

    }
}
