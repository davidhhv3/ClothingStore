using ClothingStore.Core.Entities;

namespace ClothingStore.Core.Interfaces
{
    public interface IContryService
    {
        List<Country> GetCountries();

        Task<Country> GetCountry(int Id);

        Task InsertCountry(Country country);

        Task<bool> UpdateCountry(Country country);

        Task<bool> DeleteCountry(int id);
    }

}
