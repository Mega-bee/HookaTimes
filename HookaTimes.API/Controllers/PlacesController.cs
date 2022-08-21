using HookaTimes.BLL.IServices;
using HookaTimes.BLL.ViewModels;
using HookaTimes.BLL.ViewModels.Website;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HookaTimes.API.Controllers
{
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]

    public class PlacesController : APIBaseController
    {
        private readonly IHookaPlaceBL _hookaPlaceBL;

        public PlacesController(IHookaPlaceBL hookaPlaceBL)
        {
            _hookaPlaceBL = hookaPlaceBL;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPlaces()
        {
            //string uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            int userBuddyId = Convert.ToInt32(identity.FindFirst("BuddyID").Value);
            return Ok(await _hookaPlaceBL.GetHookaPlaces(Request,userBuddyId));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlace([FromRoute] int id)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            int userBuddyId = Convert.ToInt32(identity.FindFirst("BuddyID").Value);
            return Ok(await _hookaPlaceBL.GetHookaPlace(Request, userBuddyId, id));
        }

        [HttpPut("{placeId}")]
        public async Task<IActionResult> ToggleFavorite([FromRoute] int placeId)
        {
            string uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(await _hookaPlaceBL.AddToFavorites(uid, placeId));
        }


        [HttpPost("{placeId}")]
        public async Task<IActionResult> AddReview([FromRoute] int placeId, [FromForm] CreateReview_VM review)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            int userBuddyId = Convert.ToInt32(identity.FindFirst("BuddyID").Value);
            //HookaPlaceReview_VM review = new HookaPlaceReview_VM();
            return Ok(await _hookaPlaceBL.AddReview(review, Request, placeId, userBuddyId));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreatePlace([FromForm]CreateHookaPlace_vM model)
        {
            string uid = null;
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity!.IsAuthenticated)
            {
                uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }
            return Ok(await _hookaPlaceBL.CreatePlace(model,uid));
        }


    }
}
