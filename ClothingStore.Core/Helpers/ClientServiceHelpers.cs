using ClothingStore.Core.Entities;
using ClothingStore.Core.Exceptions;
using ClothingStore.Core.Interfaces;

namespace ClothingStore.Core.Helpers
{
    internal static class ClientServiceHelpers
    {
        internal static async Task<Client> VerifyClientExistence(int id, IUnitOfWork _unitOfWork)
        {
            Client? client = await _unitOfWork.ClientRepository.GetById(id);
            if (client == null)
                throw new BusinessException("El cliente no está registrado");
            return client;
        }
    }
}
