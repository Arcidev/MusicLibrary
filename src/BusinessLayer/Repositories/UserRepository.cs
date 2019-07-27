using DataLayer.Entities;
using Riganti.Utils.Infrastructure.Core;
using System.Linq;

namespace BusinessLayer.Repositories
{
    public class UserRepository : BaseRepository<User, int>
    {
        public UserRepository(IUnitOfWorkProvider provider, IDateTimeProvider dateTimeProvider) : base(provider, dateTimeProvider) { }

        public User GetByEmail(string email)
        {
            return Context.Users.FirstOrDefault(x => x.Email == email);
        }
    }
}
