using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BL.Repositories
{
    public class UserRepository : BaseRepository<User, int>
    {
        public UserRepository(IUnitOfWorkProvider provider) : base(provider) { }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await Context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public User GetByEmail(string email)
        {
            return Context.Users.FirstOrDefault(x => x.Email == email);
        }
    }
}
