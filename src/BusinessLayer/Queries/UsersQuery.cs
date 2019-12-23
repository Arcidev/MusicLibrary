using BusinessLayer.DTO;
using Mapster;
using Riganti.Utils.Infrastructure.Core;
using System.Linq;

namespace BusinessLayer.Queries
{
    public class UsersQuery : AppQuery<UserInfoDTO>
    {
        public string Filter { get; set; }

        public UsersQuery(IUnitOfWorkProvider provider) : base(provider) { }

        protected override IQueryable<UserInfoDTO> GetQueryable()
        {
            var query = Context.Users.AsQueryable();
            if (!string.IsNullOrEmpty(Filter))
                query = query.Where(x => x.Email.Contains(Filter) || x.FirstName.Contains(Filter) || x.LastName.Contains(Filter));

            return query.ProjectToType<UserInfoDTO>();
        }
    }
}
