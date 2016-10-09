using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;

namespace BL.Repositories
{
    public class AlbumRepository : BaseRepository<Album, int>
    {
        public AlbumRepository(IUnitOfWorkProvider provider) : base(provider) { }
    }
}
