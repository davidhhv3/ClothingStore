using ClothingStore.Core.CustomEntities;
using ClothingStore.Core.Entities;
using ClothingStore.Core.Exceptions;
using ClothingStore.Core.Helpers;
using ClothingStore.Core.Interfaces;
using ClothingStore.Core.QueryFilters;
using Microsoft.Extensions.Options;

namespace ClothingStore.Core.Services
{
    public class ClientService : IClientService
    {        
        private readonly IUnitOfWork _unitOfWork;
        private readonly PaginationOptions _paginationOptions;

        public ClientService(IUnitOfWork unitOfWork, IOptions<PaginationOptions> options)
        {
            _unitOfWork = unitOfWork;
            _paginationOptions = options.Value;
        }

        public async Task<bool> DeleteClient(int id)
        {
            await ClientServiceHelpers.VerifyClientExistence(id, _unitOfWork);
            await _unitOfWork.ClientRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<Client?> GetClient(int Id)
        {
            Client? client = await ClientServiceHelpers.VerifyClientExistence(Id, _unitOfWork);
            return client;
        }

        public async Task<PagedList<Client>> GetClients(ClientQueryFilter filters)
        {       
            filters = ClientServiceHelpers.SetValueFilter(filters, _paginationOptions);
            List<Client> clients = await ClientServiceHelpers.VerifyClientsGetClients(_unitOfWork);     
            PagedList<Client> pagedClients = PagedList<Client>.Create(clients, filters.PageNumber, filters.PageSize);
            return pagedClients;
        }
        public async Task<PagedList<Client>> GetClientsByCountry(ClientQueryFilter filters,int countryId)
        {
            filters = ClientServiceHelpers.SetValueFilter(filters, _paginationOptions);
            List<Client> clients = await ClientServiceHelpers.VerifyClientsGetClients(_unitOfWork);
            List<Client> clientsByCountry = clients.Where(c => c.Country == countryId).ToList();
            PagedList<Client> pagedClients = PagedList<Client>.Create(clientsByCountry, filters.PageNumber, filters.PageSize);
            return pagedClients;
        }

        public async Task<bool> InsertCLient(Client client)
        {
            await CountryServiceHelpers.VerifyCityExistence(client.Country, _unitOfWork);               
            await _unitOfWork.ClientRepository.Add(client);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateClient(Client client)
        {
            await CountryServiceHelpers.VerifyCityExistence(client.Country, _unitOfWork);
            Client? existingClient = await ClientServiceHelpers.VerifyClientExistence(client.Id, _unitOfWork); 
            if(existingClient != null)
            {
                existingClient.Name = existingClient.Name;
                await _unitOfWork.ClientRepository.Update(existingClient);
            }        
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
