using ClothingStore.Core.Entities;
using ClothingStore.Core.Interfaces;
using ClothingStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClothingStore.Infrastructure.Repositories
{
    public class SecurityRepository : BaseRepository<Security>, ISecurityRepository
    {
        public SecurityRepository(ClothingStoreContext context) : base(context) { }

        public async Task<Security?> GetLoginByCredentials(UserLogin login)
        {           
            return await _entities.FirstOrDefaultAsync(x => x.User == login.User);
        }
    }

}
