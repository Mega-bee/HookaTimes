using HookaTimes.BLL.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookaTimes.BLL.IServices
{
    public interface IHookaNotificationBL
    {
        Task<ResponseModel> GetSentNotification(HttpRequest request, int buddyId);

    }
}
