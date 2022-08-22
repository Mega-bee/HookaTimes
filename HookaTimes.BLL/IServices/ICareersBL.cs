using HookaTimes.BLL.ViewModels.Website;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HookaTimes.BLL.IServices
{
    public interface ICareersBL
    {
        Task<List<JobVacancy_VM>> GetJobVacancies();
    }
}