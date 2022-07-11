using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HookaTimes.DAL.Models;
using HookaTimes.DAL.Services;

namespace HookaTimes.DAL.Repos
{
    class ProfileRepos : GenericRepos<AccProfile>, IProfileRepos
    {
        public ProfileRepos(SentinelDbContext context) : base(context)
        {
        }
    }
}
