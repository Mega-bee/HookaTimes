using HookaTimes.DAL.Data;
using HookaTimes.DAL.HookaTimesModels;
using HookaTimes.DAL.Services;

namespace HookaTimes.DAL.Repos
{
    public class PlaceOfferRepo : GenericRepos<PlaceOffer>, IPlaceOfferRepository
    {
        public PlaceOfferRepo(HookaDbContext context) : base(context)
        {
        }
    }
}
