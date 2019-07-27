using DataLayer.Entities;
using Riganti.Utils.Infrastructure.Core;

namespace BusinessLayer.Repositories
{
    public class StorageFileRepository : BaseRepository<StorageFile, int>
    {
        public StorageFileRepository(IUnitOfWorkProvider provider, IDateTimeProvider dateTimeProvider) : base(provider, dateTimeProvider) { }
    }
}
