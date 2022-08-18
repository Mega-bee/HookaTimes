using HookaTimes.BLL.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HookaTimes.BLL.IServices
{
    public interface ICartBL
    {
        Task<ResponseModel> AddToCart(int userBuddyId, int quantity, int productId);
        Task<ResponseModel> GetCartSummary(HttpRequest request, int userBuddyId);
        Task<ResponseModel> AddToCartCookies(string cartSessionId, int productId, int quantity);
        Task<CartSummary_VM> GetCartSummaryMVC(int userBuddyId, string cartSessionId);
        Task<ResponseModel> RemoveItemFromCart(int productId, int userBuddyId, string cartSessionId);
        Task<ResponseModel> UpdateCart(List<UpdateCartItem_VM> items, int userBuddyId, string cartSessionId);
    }
}