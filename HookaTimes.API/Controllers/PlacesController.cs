using HookaTimes.BLL.IServices;
using HookaTimes.BLL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            return Ok(await _hookaPlaceBL.GetHookaPlaces(Request));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlace([FromRoute] int id)
        {
            return Ok(await _hookaPlaceBL.GetHookaPlace(Request, id));
        }

        [HttpPut("{placeId}")]
        public async Task<IActionResult> ToggleFavorite([FromRoute] int placeId)
        {
            string uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(await _hookaPlaceBL.AddToFavorites(uid, placeId));
        }

  


    }
}
