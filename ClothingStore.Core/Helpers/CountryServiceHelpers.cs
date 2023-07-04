using ClothingStore.Core.Entities;
using ClothingStore.Core.Interfaces;

namespace ClothingStore.Core.Helpers
{
    internal static class CountryServiceHelpers
    {
        internal static async Task<Country?> VerifyCityExistence(int id, IUnitOfWork _unitOfWork)
        {
            Country? country = await _unitOfWork.CountryRepository.GetById(id);
            ObjectVerifier.VerifyExistence(country, "El pais no está registrado");
            return country;
        }
    }
}
