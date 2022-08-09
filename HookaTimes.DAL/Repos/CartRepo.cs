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
    public class CartRepo : GenericRepos<Cart>, ICartRepository
    {
        public CartRepo(HookaDbContext context) : base(context)
        {
        }
    }
}
