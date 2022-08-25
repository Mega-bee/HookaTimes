using HookaTimes.BLL.ViewModels;
using System.Threading.Tasks;

namespace HookaTimes.BLL.IServices
{
    public interface INotificationBL
    {
        Task<bool> SendNotification(NotificationModel notificationModel);
        Task<ResponseModel> GetNotifications(int userBuddyId);
    }
}