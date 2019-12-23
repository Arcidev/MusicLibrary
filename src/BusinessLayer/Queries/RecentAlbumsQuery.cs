using BusinessLayer.DTO;
using Mapster;
using Riganti.Utils.Infrastructure.Core;
using System.Linq;

namespace BusinessLayer.Queries
{
    public class RecentAlbumsQuery : AppQuery<AlbumDTO>
    {
        public RecentAlbumsQuery(IUnitOfWorkProvider provider) : base(provider) { }

        protected override IQueryable<AlbumDTO> GetQueryable()
        {
            return Context.Albums.Where(x => x.Approved).OrderByDescending(x => x.CreateDate).ProjectToType<AlbumDTO>();
        }
    }
}
