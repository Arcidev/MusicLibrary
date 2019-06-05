using AutoMapper;
using AutoMapper.QueryableExtensions;
using BL.DTO;
using Riganti.Utils.Infrastructure.Core;
using System.Linq;

namespace BL.Queries
{
    public class UserAlbumsQuery : AppQuery<AlbumDTO>
    {
        public int UserId { get; set; }

        public UserAlbumsQuery(IUnitOfWorkProvider provider, IConfigurationProvider config) : base(provider, config) { }

        protected override IQueryable<AlbumDTO> GetQueryable()
        {
            return Context.UserAlbums.Where(x => x.UserId == UserId).Select(x => x.Album).ProjectTo<AlbumDTO>(mapperConfig);
        }
    }
}
