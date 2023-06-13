using ClothingStore.Core.Entities;

namespace ClothingStore.Core.Interfaces
{
    public interface ISecurityRepository : IRepository<Security>
    {
        Task<Security> GetLoginByCredentials(UserLogin login);
    }

}
