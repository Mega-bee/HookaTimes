using HookaTimes.BLL.IServices;
using HookaTimes.BLL.ViewModels.Website;
using Microsoft.AspNetCore.Mvc;

namespace HookaTimes.MVC.Controllers
{
    public class CareersController : Controller
    {
        private readonly ICareersBL _careersBL;

        public CareersController(ICareersBL careersBL)
        {
            _careersBL = careersBL;
        }

        public async Task<IActionResult> Index()
        {
            List<JobVacancy_VM> careers = await _careersBL.GetJobVacancies();
            return View(careers);
        }
    }
}
