using HookaTimes.BLL.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace HookaTimes.BLL.IServices
{
    public interface IProductBL
    {
        Task<ResponseModel> GetAllCategories(HttpRequest request);
        Task<ResponseModel> GetProductsByCategoryId(int id, HttpRequest request);
    }
}