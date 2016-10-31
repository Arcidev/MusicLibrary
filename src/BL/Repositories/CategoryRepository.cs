using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;

namespace BL.Repositories
{
    public class CategoryRepository : BaseRepository<Category, int>
    {
        public CategoryRepository(IUnitOfWorkProvider provider) : base(provider) { }
    }
}
