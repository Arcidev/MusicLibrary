using AutoMapper;
using AutoMapper.QueryableExtensions;
using Riganti.Utils.Infrastructure.Core;
using System.Linq;

namespace BL.Queries
{
    public class BandsQuery<T> : AppQuery<T>
    {
        public BandsQuery(IUnitOfWorkProvider provider, IConfigurationProvider config) : base(provider, config) { }

        public bool? Approved { get; set; }

        public string Filter { get; set; }

        protected override IQueryable<T> GetQueryable()
        {
            var query = Context.Bands.AsQueryable();
            if (Approved.HasValue)
                query = query.Where(x => x.Approved == Approved.Value);

            if (!string.IsNullOrEmpty(Filter))
                query = query.Where(x => x.Name.Contains(Filter));

            return query.ProjectTo<T>(mapperConfig);
        }
    }
}
