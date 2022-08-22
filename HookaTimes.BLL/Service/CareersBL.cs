using AutoMapper;
using HookaTimes.BLL.IServices;
using HookaTimes.BLL.Utilities;
using HookaTimes.BLL.ViewModels.Website;
using HookaTimes.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookaTimes.BLL.Service
{
    public class CareersBL : BaseBO, ICareersBL
    {
        public CareersBL(IUnitOfWork unit, IMapper mapper, NotificationHelper notificationHelper) : base(unit, mapper, notificationHelper)
        {
        }

        public async Task<List<JobVacancy_VM>> GetJobVacancies()
        {
            List<JobVacancy_VM> careers = await _uow.JobVacancyRepository.GetAll(x=> x.IsDeleted == false).Select(x => new JobVacancy_VM
            {
                Description = x.Description,
                Id = x.Id,
                Title = x.Title
            }).ToListAsync();
            return careers;
        }
    }
}
