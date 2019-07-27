using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessLayer.DTO;
using Riganti.Utils.Infrastructure.Core;
using System.Linq;

namespace BusinessLayer.Queries
{
    public class FeaturedAlbumsQuery : AppQuery<AlbumDTO>
    {
        public FeaturedAlbumsQuery(IUnitOfWorkProvider provider, IConfigurationProvider config) : base(provider, config) { }

        protected override IQueryable<AlbumDTO> GetQueryable()
        {
            return Context.Albums.Where(x => x.Approved).OrderByDescending(x => x.AverageQuality).ProjectTo<AlbumDTO>(mapperConfig);
        }
    }
}
