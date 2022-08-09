using HookaTimes.BLL.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace HookaTimes.BLL.IServices
{
    public interface IHookaPlaceBL
    {
        Task<ResponseModel> AddToFavorites(string uid, int placeId);
        Task<ResponseModel> GetHookaPlace(HttpRequest request, int userBuddyId, int id);
        Task<ResponseModel> GetHookaPlaces(HttpRequest request);
        Task<ResponseModel> AddReview(CreateReview_VM model, HttpRequest request, int id, int buddyId);
    }
}