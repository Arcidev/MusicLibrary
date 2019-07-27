using DataLayer.Entities;
using Riganti.Utils.Infrastructure.Core;

namespace BusinessLayer.Repositories
{
    public class SliderImageRepository : BaseRepository<SliderImage, int>
    {
        public SliderImageRepository(IUnitOfWorkProvider provider, IDateTimeProvider dateTimeProvider) : base(provider, dateTimeProvider) { }
    }
}
