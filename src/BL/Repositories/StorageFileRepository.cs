using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;

namespace BL.Repositories
{
    public class StorageFileRepository : BaseRepository<StorageFile, int>
    {
        public StorageFileRepository(IUnitOfWorkProvider provider) : base(provider) { }
    }
}
