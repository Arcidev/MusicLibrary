using BusinessLayer.DTO;
using Mapster;
using Riganti.Utils.Infrastructure.Core;
using System.Linq;

namespace BusinessLayer.Queries
{
    public class SliderImagesQuery : AppQuery<SliderImageDTO>
    {
        public SliderImagesQuery(IUnitOfWorkProvider provider) : base(provider) { }

        protected override IQueryable<SliderImageDTO> GetQueryable()
        {
            return Context.SliderImages.ProjectToType<SliderImageDTO>();
        }
    }
}
