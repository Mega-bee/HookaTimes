using HookaTimes.DAL.Services;

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

        void Save();
    }
}
