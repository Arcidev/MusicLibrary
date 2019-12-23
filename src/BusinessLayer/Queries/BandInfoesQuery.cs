using BusinessLayer.DTO;
using Mapster;
using Riganti.Utils.Infrastructure.Core;
using System.Linq;

namespace BusinessLayer.Queries
{
    public class BandInfoesQuery : AppQuery<BandInfoDTO>
    {
        public BandInfoesQuery(IUnitOfWorkProvider provider) : base(provider) { }

        protected override IQueryable<BandInfoDTO> GetQueryable()
        {
            return Context.Bands.Where(x => x.Approved).ProjectToType<BandInfoDTO>();
        }
    }
}
