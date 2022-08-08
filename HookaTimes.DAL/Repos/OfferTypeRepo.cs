using HookaTimes.DAL.Data;
using HookaTimes.DAL.HookaTimesModels;
using HookaTimes.DAL.Services;

namespace HookaTimes.DAL.Repos
{
    public class OfferTypeRepo : GenericRepos<OfferType>, IOfferTypeRepository
    {
        public OfferTypeRepo(HookaDbContext context) : base(context)
        {
        }
    }
}
