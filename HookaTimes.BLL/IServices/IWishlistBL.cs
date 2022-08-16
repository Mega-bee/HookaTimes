using HookaTimes.BLL.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HookaTimes.BLL.IServices
{
    public interface IWishlistBL
    {
        Task<ResponseModel> AddToWishlist(int productId, string wishlistSessionId, int userBuddyId);
        Task<List<Wishlist_VM>> GetWishlist(int userBuddyId, string wishlistSessionId);
        Task<ResponseModel> RemoveItemFromWishlist(int productId, int userBuddyId, string wishlistSessionId);
        Task<int> GetWishlistCount(int userBuddyId, string wishlistSessionId);
    }
}