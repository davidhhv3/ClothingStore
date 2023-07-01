using ClothingStore.Core.Entities;

namespace ClothingStore.Core.Interfaces
{
    public interface IClientService
    {
        //Task<PagedList<Client>> GetClients(CountryQueryFilter filters);

        Task<Client> GetClient(int Id);

        Task InsertCLient(Client client);

        Task<bool> UpdateClient(Client client);

        Task<bool> DeleteClient(int id);
    }
}
