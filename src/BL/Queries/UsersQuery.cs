using AutoMapper;
using AutoMapper.QueryableExtensions;
using BL.DTO;
using Riganti.Utils.Infrastructure.Core;
using System.Linq;

namespace BL.Queries
{
    public class UsersQuery : AppQuery<UserInfoDTO>
    {
        public string Filter { get; set; }

        public UsersQuery(IUnitOfWorkProvider provider, IConfigurationProvider config) : base(provider, config) { }

        protected override IQueryable<UserInfoDTO> GetQueryable()
        {
            var query = Context.Users.AsQueryable();
            if (!string.IsNullOrEmpty(Filter))
                query = query.Where(x => x.Email.Contains(Filter) || x.FirstName.Contains(Filter) || x.LastName.Contains(Filter));

            return query.ProjectTo<UserInfoDTO>(mapperConfig);
        }
    }
}
