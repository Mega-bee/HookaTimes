using HookaTimes.DAL.Models;
using HookaTimes.DAL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HookaTimes.DAL.Repos;

namespace HookaTimes.DAL
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        protected readonly SentinelDbContext _context;

        public UnitOfWork(SentinelDbContext context)
        {
            _context = context;
        }



        #region private 

        private IProfileRepos profileRepos;


        #endregion



        #region public 
        public IProfileRepos ProfileRepos => profileRepos ?? new ProfileRepos(_context);


        #endregion





        public void Dispose()
        {
            _context.Dispose();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
