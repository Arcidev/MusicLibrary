using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;

namespace BL.Repositories
{
    public class SliderImageRepository : BaseRepository<SliderImage, int>
    {
        public SliderImageRepository(IUnitOfWorkProvider provider) : base(provider) { }
    }
}
