using AutoMapper.QueryableExtensions;
using BL.DTO;
using Riganti.Utils.Infrastructure.Core;
using System.Linq;

namespace BL.Queries
{
    public class BandsQuery : AppQuery<BandDTO>
    {
        public int BandId { get; set; }

        public int? ExcludeAlbumId { get; set; }

        public bool? Approved { get; set; }

        public BandsQuery(IUnitOfWorkProvider provider) : base(provider) { }

        protected override IQueryable<BandDTO> GetQueryable()
        {
            return Context.Bands.ProjectTo<BandDTO>();
        }
    }
}
