using AutoMapper;
using HookaTimes.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HookaTimes.BLL.Utilities;

namespace HookaTimes.BLL.Service
{
    public class BaseBO
    {
        protected readonly IUnitOfWork _uow;
        protected readonly IMapper _mapper;
        protected readonly NotificationHelper _notificationHelper;
        public BaseBO(IUnitOfWork unit, IMapper mapper, NotificationHelper notificationHelper)
        {
            _uow = unit;
            _mapper = mapper;
            _notificationHelper = notificationHelper;
        }
    }
}
