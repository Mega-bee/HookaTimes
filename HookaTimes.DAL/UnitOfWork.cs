//using HookaTimes.DAL.Models;
using HookaTimes.DAL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HookaTimes.DAL.Repos;
using HookaTimes.DAL.Data;

namespace HookaTimes.DAL
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        protected readonly HookaDbContext _context;

        public UnitOfWork(HookaDbContext context)
        {
            _context = context;
        }



        #region private 

        //private IProfileRepos profileRepos;

        private IUserRepos userRepos;
        private IPlaceReposiotry placeReposiotry;
        private IBuddyRepository buddyRepository;
        private IFavoritePlaceRepository favoritePlaceRepository;


        #endregion



        #region public 
        //public IProfileRepos ProfileRepos => profileRepos ?? new ProfileRepos(_context);
        public IUserRepos UserRepos => userRepos ?? new UserRepos(_context);
        public IPlaceReposiotry PlaceRepository => placeReposiotry ?? new PlaceRepo(_context);
        public IBuddyRepository BuddyRepository => buddyRepository ?? new BuddyRepo(_context);
        public IFavoritePlaceRepository FavoritePlaceRepository => favoritePlaceRepository ?? new FavoritePlaceRepo(_context);


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
