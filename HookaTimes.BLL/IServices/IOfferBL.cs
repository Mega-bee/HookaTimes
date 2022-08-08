using HookaTimes.BLL.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace HookaTimes.BLL.IServices
{
    public interface IOfferBL
    {
        Task<ResponseModel> GetOfferList(HttpRequest request);
        Task<ResponseModel> GetOfferById(int id, HttpRequest request);

    }
}