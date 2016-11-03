using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;

namespace BL.Repositories
{
    public class UserAlbumRepository : BaseRepository<UserAlbum, int>
    {
        public UserAlbumRepository(IUnitOfWorkProvider provider) : base(provider) { }
    }
}
