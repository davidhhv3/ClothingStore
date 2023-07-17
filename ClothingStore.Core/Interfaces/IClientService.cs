using ClothingStore.Core.CustomEntities;
using ClothingStore.Core.Entities;
using ClothingStore.Core.QueryFilters;

namespace ClothingStore.Core.Interfaces
{
    public interface IClientService
    {
        Task<PagedList<Client>> GetClients(ClientQueryFilter filters);

        Task<PagedList<Client>> GetClientsByCountry(ClientQueryFilter filters,int countryId);

        Task<Client?> GetClient(int Id);

        Task<bool> InsertCLient(Client client);

        Task<bool> UpdateClient(Client client);

        Task<bool> DeleteClient(int id);
    }
}
