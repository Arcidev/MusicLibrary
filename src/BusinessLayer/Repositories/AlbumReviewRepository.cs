using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Riganti.Utils.Infrastructure.Core;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLayer.Repositories
{
    public class AlbumReviewRepository : BaseRepository<AlbumReview, int>
    {
        public AlbumReviewRepository(IUnitOfWorkProvider provider, IDateTimeProvider dateTimeProvider) : base(provider, dateTimeProvider) { }

        public async Task<double> GetAlbumAverageQualityAsync(int albumId)
        {
            return await Context.AlbumReviews.Where(x => x.AlbumId == albumId).AverageAsync(x => (int)x.Quality);
        }
    }
}
