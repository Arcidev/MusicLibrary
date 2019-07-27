using DataLayer.Entities;
using Riganti.Utils.Infrastructure.Core;
using System.Linq;

namespace BusinessLayer.Repositories
{
    public class AlbumReviewRepository : BaseRepository<AlbumReview, int>
    {
        public AlbumReviewRepository(IUnitOfWorkProvider provider, IDateTimeProvider dateTimeProvider) : base(provider, dateTimeProvider) { }

        public double GetAlbumAverageQuality(int albumId)
        {
            return Context.AlbumReviews.Where(x => x.AlbumId == albumId).Average(x => (int)x.Quality);
        }
    }
}
