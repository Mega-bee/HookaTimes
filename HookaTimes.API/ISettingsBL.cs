using HookaTimes.BLL.ViewModels;
using System.Threading.Tasks;

namespace HookaTimes.BLL.Service
{
    public interface ISettingsBL
    {
        Task<ResponseModel> GetSettings(int userBuddyId);
    }
}