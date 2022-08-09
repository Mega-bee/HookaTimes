using HookaTimes.BLL.ViewModels;
using System.Threading.Tasks;

namespace HookaTimes.BLL.IServices
{
    public interface ICartBL
    {
        Task<ResponseModel> AddToCart(int userBuddyId, int quantity, int productId);
    }
}