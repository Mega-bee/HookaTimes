using HookaTimes.BLL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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
