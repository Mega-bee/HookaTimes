using HookaTimes.BLL.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace HookaTimes.BLL.IServices
{
    public interface IOrderBL
    {
        Task<ResponseModel> PlaceOrder(int userBuddyId, int addressId, BuddyProfileAddressVM address=null);
        Task<ResponseModel> GetOrders(int userBuddyId);
        Task<ResponseModel> GetOrder(HttpRequest request, int userBuddyId, int orderId);
    }
}