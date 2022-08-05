using HookaTimes.BLL.ViewModels;
using System.Threading.Tasks;

namespace HookaTimes.BLL.IServices
{
    public interface IInvitationBL
    {
        Task<ResponseModel> SetInvitationStatus(int statusId, int invitationId);
    }
}