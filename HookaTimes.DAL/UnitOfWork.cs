//using HookaTimes.DAL.Models;
using HookaTimes.DAL.Data;
using HookaTimes.DAL.Repos;
using HookaTimes.DAL.Services;
using System;
using System.Threading.Tasks;

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
        private IPlaceReviewRepo placeReviewRepository;
        private IInvitationRepository invitationRepository;
        private IInvitationOptionRepository invitationOptionRepository;
        private IPlaceOfferRepository placeOfferRepository;
        private IOfferTypeRepository offerTypeRepository;
        private IProductCategoryRepository productCategoryRepository;
        private IProductRepository productRepository;
        private ICartRepository cartRepository;
        private IBuddyProfileAddressRepository buddyProfileAddressRepository;
        private IBuddyProfileEducationRepository buddyProfileEducationRepository;
        private IBuddyProfileExperienceRepository buddyProfileExperienceRepository;


        #endregion



        #region public 
        //public IProfileRepos ProfileRepos => profileRepos ?? new ProfileRepos(_context);
        public IUserRepos UserRepos => userRepos ?? new UserRepos(_context);
        public IPlaceReposiotry PlaceRepository => placeReposiotry ?? new PlaceRepo(_context);
        public IBuddyRepository BuddyRepository => buddyRepository ?? new BuddyRepo(_context);
        public IFavoritePlaceRepository FavoritePlaceRepository => favoritePlaceRepository ?? new FavoritePlaceRepo(_context);
        public IPlaceReviewRepo PlaceReviewRepository => placeReviewRepository ?? new PlaceReviewRepo(_context);
        public IInvitationRepository InvitationRepository => invitationRepository ?? new InvitationRepo(_context);
        public IInvitationOptionRepository InvitationOptionRepository => invitationOptionRepository ?? new InvitationOptionRepo(_context);
        public IPlaceOfferRepository PlaceOfferRepository => placeOfferRepository ?? new PlaceOfferRepo(_context);
        public IOfferTypeRepository OfferTypeRepository => offerTypeRepository ?? new OfferTypeRepo(_context);
        public IProductCategoryRepository ProductCategoryRepository => productCategoryRepository ?? new ProductCategoryRepo(_context);
        public IProductRepository ProductRepository => productRepository ?? new ProductRepo(_context);
        public ICartRepository CartRepository => cartRepository ?? new CartRepo(_context);
        public IBuddyProfileAddressRepository BuddyProfileAddressRepository => buddyProfileAddressRepository ?? new BuddyProfileAddressRepo(_context);
        public IBuddyProfileEducationRepository BuddyProfileEducationRepository => buddyProfileEducationRepository ?? new BuddyProfileEducationRepo(_context);
        public IBuddyProfileExperienceRepository BuddyProfileExperienceRepository => buddyProfileExperienceRepository ?? new BuddyProfileExperienceRepo(_context);


        #endregion





        public void Dispose()
        {
            _context.Dispose();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async virtual Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
