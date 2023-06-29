using ClothingStore.Core.Entities;
using ClothingStore.Core.Exceptions;
using ClothingStore.Core.Interfaces;

namespace ClothingStore.Core.Helpers
{
    internal static class CountryServiceHelpers
    {
        internal static void CheckForbiddenWords(string Name)
        {
            List<string> forbiddenWords = new List<string> { "sexo", "pechos" };
            foreach (string word in forbiddenWords)
                if (Name.Contains(word, StringComparison.OrdinalIgnoreCase))
                    throw new BusinessException("Contenido no permitido");
        }
        internal static async Task<Country> VerifyCityExistence(int id, IUnitOfWork _unitOfWork)
        {
            Country? country = await _unitOfWork.CountryRepository.GetById(id);
            if (country == null)
                throw new BusinessException("La ciudad no está registrada");
            return country;
        }
    }
}
