using HookaTimes.BLL.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace HookaTimes.BLL.IServices
{
    public interface IHookaBuddyBL
    {
        Task<ResponseModel> GetBuddies(HttpRequest request, string uid);
        Task<ResponseModel> InviteBuddy(int userBuddyId, Invitation_VM model);
    }
}