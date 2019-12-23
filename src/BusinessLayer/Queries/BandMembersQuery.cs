using BusinessLayer.DTO;
using Mapster;
using Riganti.Utils.Infrastructure.Core;
using System.Linq;

namespace BusinessLayer.Queries
{
    public class BandMembersQuery : AppQuery<ArtistDTO>
    {
        public int BandId { get; set; }

        public bool? Approved { get; set; }

        public BandMembersQuery(IUnitOfWorkProvider provider) : base(provider) { }

        protected override IQueryable<ArtistDTO> GetQueryable()
        {
            var query = Context.BandMembers.Where(x => x.BandId == BandId).Select(x => x.Artist);
            if (Approved.HasValue)
                query = query.Where(x => x.Approved == Approved.Value);

            return query.ProjectToType<ArtistDTO>();
        }
    }
}
