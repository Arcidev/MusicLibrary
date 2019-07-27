using DataLayer.Entities;
using Riganti.Utils.Infrastructure.Core;
using System.Linq;

namespace BusinessLayer.Repositories
{
    public class UserAlbumRepository : BaseRepository<UserAlbum, int>
    {
        public UserAlbumRepository(IUnitOfWorkProvider provider, IDateTimeProvider dateTimeProvider) : base(provider, dateTimeProvider) { }

        public UserAlbum GetUserAlbum(int userId, int albumId)
        {
            return Context.UserAlbums.FirstOrDefault(x => x.AlbumId == albumId && x.UserId == userId);
        }
    }
}
