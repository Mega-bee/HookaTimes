using HookaTimes.BLL.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HookaTimes.BLL.IServices
{
    public interface ICuisineBL
    {
        Task<List<Cuisine_VM>> GetCuisinesMVC();
    }
}