using AutoMapper.QueryableExtensions;
using Riganti.Utils.Infrastructure.Core;
using System.Linq;

namespace BL.Queries
{
    public class SongsQuery<T> : AppQuery<T>
    {
        public SongsQuery(IUnitOfWorkProvider provider) : base(provider) { }

        protected override IQueryable<T> GetQueryable()
        {
            return Context.Songs.Where(x => x.Approved).ProjectTo<T>();
        }
    }
}
