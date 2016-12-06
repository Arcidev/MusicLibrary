using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;

namespace BL.Repositories
{
    public class BandReviewRepository : BaseRepository<BandReview, int>
    {
        public BandReviewRepository(IUnitOfWorkProvider provider) : base(provider) { }
    }
}
