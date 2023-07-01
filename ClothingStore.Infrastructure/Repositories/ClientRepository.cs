using ClothingStore.Core.Entities;
using ClothingStore.Core.Interfaces;
using ClothingStore.Infrastructure.Data;

namespace ClothingStore.Infrastructure.Repositories
{
    internal class ClientRepository : BaseRepository<Client>, IClientRepository
    {
        public ClientRepository(ClothingStoreContext context) : base(context) { }
    }
}
