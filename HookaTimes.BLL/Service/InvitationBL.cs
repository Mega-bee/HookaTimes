using AutoMapper;
using HookaTimes.BLL.Utilities;
using HookaTimes.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookaTimes.BLL.Service
{
    public class InvitationBL : BaseBO
    {
        public InvitationBL(IUnitOfWork unit, IMapper mapper, NotificationHelper notificationHelper) : base(unit, mapper, notificationHelper)
        {
        }
    }
}
