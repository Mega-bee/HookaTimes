﻿using HookaTimes.DAL.Services;
using System.Threading.Tasks;

namespace HookaTimes.DAL
{
    public interface IUnitOfWork
    {
        //IProfileRepos ProfileRepos { get; }
        IUserRepos UserRepos { get; }
        IPlaceReposiotry PlaceRepository { get; }
        IBuddyRepository BuddyRepository { get; }
        IFavoritePlaceRepository FavoritePlaceRepository { get; }

        IPlaceReviewRepo PlaceReviewRepository { get; }
        IInvitationRepository InvitationRepository { get; }
        IInvitationOptionRepository InvitationOptionRepository { get; }

        IPlaceOfferRepository PlaceOfferRepository { get; }

        IOfferTypeRepository OfferTypeRepository { get; }
        IProductRepository ProductRepository { get; }
        IProductCategoryRepository ProductCategoryRepository { get; }
        ICartRepository CartRepository { get; }
        IBuddyProfileAddressRepository BuddyProfileAddressRepository { get; }
        IBuddyProfileEducationRepository BuddyProfileEducationRepository { get; }
        IBuddyProfileExperienceRepository BuddyProfileExperienceRepository { get; }
        IOrderRepository OrderRepository { get; }
        IOrderItemRepository OrderItemRepository { get; }
        ICuisineRepository CuisineRepository { get; }
        IVirtualCartRepository VirtualCartRepository { get; }
        void Save();
        Task SaveAsync();
    }
}
