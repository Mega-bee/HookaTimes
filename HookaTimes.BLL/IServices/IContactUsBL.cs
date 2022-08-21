using HookaTimes.BLL.ViewModels;
using System.Threading.Tasks;

namespace HookaTimes.BLL.IServices
{
    public interface IContactUsBL
    {
        Task<ResponseModel> SendContactUsMessage(ContactUs_VM model);
    }
}