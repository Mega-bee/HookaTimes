using HookaTimes.BLL.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace HookaTimes.BLL.IServices
{
    public interface IInvitationBL
    {
        Task<ResponseModel> SetInvitationStatus(int statusId, int invitationId);
        Task<ResponseModel> GetSentInvitations(HttpRequest request, int userBuddyId);
        Task<ResponseModel> GetInvitationOptions();
        Task<ResponseModel> GetRecievedInvitations(HttpRequest request, int userBuddyId);
    }
}