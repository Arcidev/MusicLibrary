using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Riganti.Utils.Infrastructure.Core;
using System.Threading.Tasks;

namespace BusinessLayer.Repositories
{
    public class UserAlbumRepository : BaseRepository<UserAlbum, int>
    {
        public UserAlbumRepository(IUnitOfWorkProvider provider, IDateTimeProvider dateTimeProvider) : base(provider, dateTimeProvider) { }

        public async Task<UserAlbum> GetUserAlbumAsync(int userId, int albumId)
        {
            return await Context.UserAlbums.FirstOrDefaultAsync(x => x.AlbumId == albumId && x.UserId == userId);
        }
    }
}
