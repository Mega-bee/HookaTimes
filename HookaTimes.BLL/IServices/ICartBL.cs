using HookaTimes.BLL.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace HookaTimes.BLL.IServices
{
    public interface ICartBL
    {
        Task<ResponseModel> AddToCart(int userBuddyId, int quantity, int productId);
        Task<ResponseModel> GetCartSummary(HttpRequest request, int userBuddyId);
    }
}