using HookaTimes.DAL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        ISentNotificationRepo SentNotificationsRepository { get; }
        IInvDetailRepo InvDetailRepository { get; }

        void Save();
    }
}
