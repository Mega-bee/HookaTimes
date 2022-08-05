using HookaTimes.DAL.Data;
using HookaTimes.DAL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookaTimes.DAL.Repos
{
    public class PlaceReviewRepo : GenericRepos<PlaceReviewRepo>, IPlaceReviewRepo
    {
        public PlaceReviewRepo(HookaDbContext context) : base(context)
        {
        }
    }
}
