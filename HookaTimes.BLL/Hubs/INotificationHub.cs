using System.Threading.Tasks;


namespace HookaTimes.BLL.Hubs
{
    public interface INotificationHub
    {
        Task MessageToUser(object outgoingMessage);
        Task UpdatedUserList(object onlineUsers);
        Task UpdatedDashboard(dynamic patients);
    }
}
