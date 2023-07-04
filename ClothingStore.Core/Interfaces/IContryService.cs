using ClothingStore.Core.CustomEntities;
using ClothingStore.Core.Entities;
using ClothingStore.Core.QueryFilters;

namespace ClothingStore.Core.Interfaces
{
    public interface IContryService
    {
        Task<PagedList<Country>> GetCountries(CountryQueryFilter filters);

        Task<Country?> GetCountry(int Id);

        Task<bool> InsertCountry(Country country);

        Task<bool> UpdateCountry(Country country);

        Task<bool> DeleteCountry(int id);
    }

}
