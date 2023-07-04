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
            filters.PageNumber = filters.PageNumber == 0 ? _paginationOptions.DefaultPageNumber : filters.PageNumber;
            filters.PageSize = filters.PageSize == 0 ? _paginationOptions.DefaultPageSize : filters.PageSize;
            List<Client> clients  = (await _unitOfWork.ClientRepository.GetAll()).ToList();
            if (clients.Count == 0 || clients == null)
                throw new BusinessException("Aún no hay clientes registrados");
            PagedList<Client> pagedClients = PagedList<Client>.Create(clients, filters.PageNumber, filters.PageSize);
            return pagedClients;
        }

        public async Task InsertCLient(Client client)
        {
            Client? existingClient = await _unitOfWork.ClientRepository.GetById(client.Id);
            if (existingClient != null)
                throw new BusinessException("El cliente ya está registrado");
            await _unitOfWork.ClientRepository.Add(client);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> UpdateClient(Client client)
        {
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
