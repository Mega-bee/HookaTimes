using HookaTimes.BLL.ViewModels;
using System.Threading.Tasks;

namespace HookaTimes.BLL.IServices
{
    public interface IOrderBL
    {
        Task<ResponseModel> PlaceOrder(int userBuddyId, int addressId);
    }
}