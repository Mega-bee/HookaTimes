using AutoMapper;
using HookaTimes.BLL.IServices;
using HookaTimes.BLL.Utilities;
using HookaTimes.BLL.ViewModels;
using HookaTimes.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookaTimes.BLL.Service
{
    public class CuisineBL : BaseBO, ICuisineBL
    {
        public CuisineBL(IUnitOfWork unit, IMapper mapper, NotificationHelper notificationHelper) : base(unit, mapper, notificationHelper)
        {
        }

        public async Task<List<Cuisine_VM>> GetCuisinesMVC()
        {
            List<Cuisine_VM> cuisines = await _uow.CuisineRepository.GetAll(c => c.IsDeleted == false).Select(c => new Cuisine_VM
            {
                Id = c.Id,
                Title = c.Title
            }).ToListAsync();
            return cuisines;
        }
    }
}
