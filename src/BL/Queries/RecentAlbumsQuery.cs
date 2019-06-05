using AutoMapper;
using AutoMapper.QueryableExtensions;
using BL.DTO;
using Riganti.Utils.Infrastructure.Core;
using System.Linq;

namespace BL.Queries
{
    public class RecentAlbumsQuery : AppQuery<AlbumDTO>
    {
        public RecentAlbumsQuery(IUnitOfWorkProvider provider, IConfigurationProvider config) : base(provider, config) { }

        protected override IQueryable<AlbumDTO> GetQueryable()
        {
            return Context.Albums.Where(x => x.Approved).OrderByDescending(x => x.CreateDate).ProjectTo<AlbumDTO>(mapperConfig);
        }
    }
}
