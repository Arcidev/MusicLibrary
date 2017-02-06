using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;
using System.Linq;

namespace BL.Repositories
{
    public class AlbumReviewRepository : BaseRepository<AlbumReview, int>
    {
        public AlbumReviewRepository(IUnitOfWorkProvider provider) : base(provider) { }

        public double GetAlbumAverageQuality(int albumId)
        {
            return Context.AlbumReviews.Where(x => x.AlbumId == albumId).Average(x => (int)x.Quality);
        }
    }
}
