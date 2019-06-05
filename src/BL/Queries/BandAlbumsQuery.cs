using AutoMapper;
using AutoMapper.QueryableExtensions;
using BL.DTO;
using Riganti.Utils.Infrastructure.Core;
using System.Linq;

namespace BL.Queries
{
    public class BandAlbumsQuery : AppQuery<AlbumDTO>
    {
        public int BandId { get; set; }

        public int? ExcludeAlbumId { get; set; }

        public bool? Approved { get; set; }

        public BandAlbumsQuery(IUnitOfWorkProvider provider, IConfigurationProvider config) : base(provider, config) { }

        protected override IQueryable<AlbumDTO> GetQueryable()
        {
            var query = Context.Albums.Where(x => x.BandId == BandId);
            if (ExcludeAlbumId.HasValue)
                query = query.Where(x => x.Id != ExcludeAlbumId.Value);
            if (Approved.HasValue)
                query = query.Where(x => x.Approved == Approved.Value);

            return query.ProjectTo<AlbumDTO>(mapperConfig);
        }
    }
}
