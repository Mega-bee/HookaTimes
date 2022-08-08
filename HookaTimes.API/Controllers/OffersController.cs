using HookaTimes.BLL.IServices;
using HookaTimes.BLL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HookaTimes.API.Controllers
{

    public class OffersController : APIBaseController
    {
        private readonly IOfferBL _offer;

        public OffersController(IOfferBL offer)
        {
            _offer = offer;
        }

        #region OfferList

        [HttpGet]
        public async Task<IActionResult> GetAllOffers()
        {
            return Ok(await _offer.GetOfferList(Request));
        }
        #endregion


        #region OfferById

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOfferById([FromRoute] int id)
        {
            ResponseModel model = await _offer.GetOfferById(id, Request);

            return Ok(model);
        }
        #endregion
    }
}
