using HookaTimes.BLL.ViewModels;
using HookaTimes.BLL.ViewModels.Website;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HookaTimes.BLL.IServices
{
    public interface IHookaPlaceBL
    {
        Task<ResponseModel> AddToFavorites(string uid, int placeId);
        Task<ResponseModel> GetHookaPlace(HttpRequest request, int userBuddyId, int id);
        Task<ResponseModel> GetHookaPlaces(HttpRequest request, int userBuddyId);
        Task<ResponseModel> AddReview(CreateReview_VM model, HttpRequest request, int id, int buddyId);
        Task<List<HookaPlaces_VM>> GetHookaPlacesMVC(HttpRequest request, int userBuddyId, int take = 0, List<int> cuisines = null, int sortBy = 0);
        Task<List<HookaPlaces_VM>> GetFavorites(int userBuddyId);
        Task<ResponseModel> CreatePlace(CreateHookaPlace_vM model, string uid);

        Task<List<PlacesNames_VM>> GetPlacesNames();
    }
}