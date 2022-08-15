using HookaTimes.BLL.ViewModels;
using System.Threading.Tasks;

namespace HookaTimes.BLL.IServices
{
    public interface IWishlistBL
    {
        Task<ResponseModel> AddToWishlist(int productId, string wishlistSessionId, int userBuddyId);
    }
}