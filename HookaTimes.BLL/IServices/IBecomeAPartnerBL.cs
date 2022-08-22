using HookaTimes.BLL.ViewModels;
using HookaTimes.BLL.ViewModels.Website;
using System.Threading.Tasks;

namespace HookaTimes.BLL.IServices
{
    public interface IBecomeAPartnerBL
    {
        Task<ResponseModel> SendPartnerRequest(BecomeAPartner_VM model);
    }
}