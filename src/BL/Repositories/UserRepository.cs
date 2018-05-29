using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;
using System.Data.Entity;
using System.Threading.Tasks;

namespace BL.Repositories
{
    public class UserRepository : BaseRepository<User, int>
    {
        public UserRepository(IUnitOfWorkProvider provider, IDateTimeProvider dateTimeProvider) : base(provider, dateTimeProvider) { }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await Context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await Context.Users.Include(x => x.ImageStorageFile).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
