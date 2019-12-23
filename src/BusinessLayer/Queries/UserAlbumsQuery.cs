using BusinessLayer.DTO;
using Mapster;
using Riganti.Utils.Infrastructure.Core;
using System.Linq;

namespace BusinessLayer.Queries
{
    public class UserAlbumsQuery : AppQuery<AlbumDTO>
    {
        public int UserId { get; set; }

        public UserAlbumsQuery(IUnitOfWorkProvider provider) : base(provider) { }

        protected override IQueryable<AlbumDTO> GetQueryable()
        {
            return Context.UserAlbums.Where(x => x.UserId == UserId).Select(x => x.Album).ProjectToType<AlbumDTO>();
        }
    }
}
