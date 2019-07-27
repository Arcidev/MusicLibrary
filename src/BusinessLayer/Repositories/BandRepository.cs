using DataLayer.Entities;
using Riganti.Utils.Infrastructure.Core;

namespace BusinessLayer.Repositories
{
    public class BandRepository : BaseRepository<Band, int>
    {
        public BandRepository(IUnitOfWorkProvider provider, IDateTimeProvider dateTimeProvider) : base(provider, dateTimeProvider) { }
    }
}
