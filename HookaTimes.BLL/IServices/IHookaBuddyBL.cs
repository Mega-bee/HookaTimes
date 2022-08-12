using HookaTimes.BLL.ViewModels;
using HookaTimes.BLL.ViewModels.Website;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HookaTimes.BLL.IServices
{
    public interface IHookaBuddyBL
    {
        Task<ResponseModel> GetBuddies(HttpRequest request, int userBuddyId, string uid);
        Task<ResponseModel> GetBuddy(int BuddyId, HttpRequest Request);
        Task<ResponseModel> InviteBuddy(int userBuddyId, SendInvitation_VM model);
        Task<List<Buddy_VM>> GetBuddiesMVC(HttpRequest request, int userBuddyId);
    }
}