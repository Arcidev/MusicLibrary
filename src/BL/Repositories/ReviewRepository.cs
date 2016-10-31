using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;

namespace BL.Repositories
{
    public class AlbumReviewRepository : BaseRepository<AlbumReview, int>
    {
        public AlbumReviewRepository(IUnitOfWorkProvider provider) : base(provider) { }
    }
}
