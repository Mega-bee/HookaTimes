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
    public class PlaceRepo : GenericRepos<PlacesProfile>, IPlaceReposiotry
    {
        public PlaceRepo(HookaDbContext context) : base(context)
        {
        }
    }
}
