using HookaTimes.DAL.Services;
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
        IVirtualWishListRepository VirtualWishlistRepository { get; }
        IWishlistRepository WishlistRepository { get; }
        IPlaceAlbumRepository PlaceAlbumRepository { get; }
        IPlaceMenuRepository PlaceMenuRepository { get; }
        IJobVacancyRepository JobVacancyRepository { get; }
        IContactUsRepository ContactUsRepository { get; }
        IPartnerRequestRepository PartnerRequestRepository { get; }
        IContactInfoRepository ContactInfoRepository { get; }
        INotificationRepo NotificationRepository { get; }
        void Save();
        Task SaveAsync();
    }
}
