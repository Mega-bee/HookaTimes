using HookaTimes.BLL.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace HookaTimes.BLL.IServices
{
    public interface ICartBL
    {
        Task<ResponseModel> AddToCart(int userBuddyId, int quantity, int productId);
        Task<ResponseModel> GetCartSummary(HttpRequest request, int userBuddyId);
        Task<ResponseModel> AddToCartCookies(string cartSessionId, int productId, int quantity);
        Task<CartSummary_VM> GetCartSummaryMVC(int userBuddyId, string cartSessionId);
    }
}