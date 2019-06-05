using AutoMapper;
using AutoMapper.QueryableExtensions;
using BL.DTO;
using Riganti.Utils.Infrastructure.Core;
using System.Linq;

namespace BL.Queries
{
    public class BandInfoesQuery : AppQuery<BandInfoDTO>
    {
        public BandInfoesQuery(IUnitOfWorkProvider provider, IConfigurationProvider config) : base(provider, config) { }

        protected override IQueryable<BandInfoDTO> GetQueryable()
        {
            return Context.Bands.Where(x => x.Approved).ProjectTo<BandInfoDTO>(mapperConfig);
        }
    }
}
