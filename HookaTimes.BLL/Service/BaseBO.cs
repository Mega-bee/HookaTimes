using AutoMapper;
using HookaTimes.BLL.Utilities;
using HookaTimes.DAL;

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
