using AutoMapper.QueryableExtensions;
using BL.DTO;
using Riganti.Utils.Infrastructure.Core;
using System.Linq;

namespace BL.Queries
{
    public class BandsQuery : AppQuery<BandDTO>
    {
        public BandsQuery(IUnitOfWorkProvider provider) : base(provider) { }

        protected override IQueryable<BandDTO> GetQueryable()
        {
            return Context.Bands.Where(x => x.Approved).ProjectTo<BandDTO>();
        }
    }
}
