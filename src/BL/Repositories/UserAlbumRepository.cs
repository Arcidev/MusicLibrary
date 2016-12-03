using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;
using System.Data.Entity;
using System.Threading.Tasks;

namespace BL.Repositories
{
    public class UserAlbumRepository : BaseRepository<UserAlbum, int>
    {
        public UserAlbumRepository(IUnitOfWorkProvider provider) : base(provider) { }

        public async Task<UserAlbum> GetUserAlbum(int userId, int albumId)
        {
            return await Context.UserAlbums.FirstOrDefaultAsync(x => x.AlbumId == albumId && x.UserId == userId);
        }
    }
}
