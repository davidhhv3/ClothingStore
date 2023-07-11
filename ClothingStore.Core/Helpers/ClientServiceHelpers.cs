using ClothingStore.Core.CustomEntities;
using ClothingStore.Core.Entities;
using ClothingStore.Core.Interfaces;
using ClothingStore.Core.QueryFilters;

namespace ClothingStore.Core.Helpers
{
    internal static class ClientServiceHelpers
    {
        internal static ClientQueryFilter SetValueFilter(ClientQueryFilter filters, PaginationOptions _paginationOptions)
        {
            filters.PageNumber = filters.PageNumber == 0 ? _paginationOptions.DefaultPageNumber : filters.PageNumber;
            filters.PageSize = filters.PageSize == 0 ? _paginationOptions.DefaultPageSize : filters.PageSize;
            return filters;
        }
        internal static async Task<Client?> VerifyClientExistence(int id, IUnitOfWork _unitOfWork)
        {
            Client? client = await _unitOfWork.ClientRepository.GetById(id);
            ObjectVerifier.VerifyExistence(client, "El cliente no está registrado");
            return client;
        }
        internal static async Task<List<Client>> VerifyClientsGetClients(IUnitOfWork _unitOfWork)
        {
            List<Client> clients = (await _unitOfWork.ClientRepository.GetAll()).ToList();      
            ObjectVerifier.VerifyExistence(clients, "Aún no hay clientes registrados",clients.Count());
            return clients;
        }
    }
}
