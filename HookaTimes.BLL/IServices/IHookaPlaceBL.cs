using HookaTimes.BLL.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace HookaTimes.BLL.IServices
{
    public interface IHookaPlaceBL
    {
        Task<ResponseModel> AddToFavorites(string uid, int placeId);
        Task<ResponseModel> GetHookaPlace(HttpRequest request, int id);
        Task<ResponseModel> GetHookaPlaces(HttpRequest request);
    }
}