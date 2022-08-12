using HookaTimes.BLL.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace HookaTimes.BLL.IServices
{
    public interface IHookaBuddyBL
    {
        Task<ResponseModel> GetBuddies(HttpRequest request, int userBuddyId, string uid);
        Task<ResponseModel> GetBuddy(int BuddyId, HttpRequest Request);
        Task<ResponseModel> InviteBuddy(int userBuddyId, SendInvitation_VM model);
    }
}