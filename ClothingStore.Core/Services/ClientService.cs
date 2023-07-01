using ClothingStore.Core.CustomEntities;
using ClothingStore.Core.Entities;
using ClothingStore.Core.Helpers;
using ClothingStore.Core.Interfaces;
using Microsoft.Extensions.Options;

namespace ClothingStore.Core.Services
{
    public class ClientService : IClientService
    {        
        private readonly IUnitOfWork _unitOfWork;

        public ClientService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;           
        }

        public Task<bool> DeleteClient(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Client> GetClient(int Id)
        {
            Client client = await ClientServiceHelpers.VerifyClientExistence(Id, _unitOfWork);
            return client;
        }

        public Task InsertCLient(Client client)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateClient(Client client)
        {
            throw new NotImplementedException();
        }
    }
}
