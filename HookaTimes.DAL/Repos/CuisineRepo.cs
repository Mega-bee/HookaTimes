using HookaTimes.DAL.Data;
using HookaTimes.DAL.HookaTimesModels;
using HookaTimes.DAL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookaTimes.DAL.Repos
{
    public class CuisineRepo : GenericRepos<Cuisine>, ICuisineRepository
    {
        public CuisineRepo(HookaDbContext context) : base(context)
        {
        }
    }
}
