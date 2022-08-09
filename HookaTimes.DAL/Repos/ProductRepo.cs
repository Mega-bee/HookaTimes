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
    public class ProductRepo : GenericRepos<Product>, IProductRepository
    {
        public ProductRepo(HookaDbContext context) : base(context)
        {
        }
    }
}
