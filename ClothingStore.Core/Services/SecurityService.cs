using ClothingStore.Core.Entities;
using ClothingStore.Core.Exceptions;
using ClothingStore.Core.Interfaces;

namespace ClothingStore.Core.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SecurityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Security> GetLoginByCredentials(UserLogin userLogin)
        {
            Security? result = await _unitOfWork.SecurityRepository.GetLoginByCredentials(userLogin);
            if (result == null)
            {                 
                throw new BusinessException("Error al iniciar sesión");
            }
            return result;
        }

        public async Task RegisterUser(Security security)
        {
            await _unitOfWork.SecurityRepository.Add(security);
            await _unitOfWork.SaveChangesAsync();
        }
    }

}
