using AutoMapper.QueryableExtensions;
using BL.DTO;
using Riganti.Utils.Infrastructure.Core;
using System.Data.Entity;
using System.Linq;

namespace BL.Queries
{
    public class SliderImagesQuery : AppQuery<SliderImageDTO>
    {
        public SliderImagesQuery(IUnitOfWorkProvider provider) : base(provider) { }

        protected override IQueryable<SliderImageDTO> GetQueryable()
        {
            return Context.SliderImages.Include(x => x.ImageStorageFile).ProjectTo<SliderImageDTO>();
        }
    }
}
