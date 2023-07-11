using ClothingStore.Core.CustomEntities;
using ClothingStore.Core.Entities;
using ClothingStore.Core.Interfaces;
using ClothingStore.Core.QueryFilters;

namespace ClothingStore.Core.Helpers
{
    internal static class CountryServiceHelpers
    {
        internal static CountryQueryFilter SetValueFilter(CountryQueryFilter filters, PaginationOptions _paginationOptions)
        {
            filters.PageNumber = filters.PageNumber == 0 ? _paginationOptions.DefaultPageNumber : filters.PageNumber;
            filters.PageSize = filters.PageSize == 0 ? _paginationOptions.DefaultPageSize : filters.PageSize;
            return filters;
        }
        internal static async Task<Country?> VerifyCityExistence(int id, IUnitOfWork _unitOfWork)
        {
            Country? country = await _unitOfWork.CountryRepository.GetById(id);
            ObjectVerifier.VerifyExistence(country, "El pais no está registrado");
            return country;
        }
        internal static async Task<List<Country>> VerifyCountriesGetCountries(IUnitOfWork _unitOfWork)
        {
            List<Country> countries = (await _unitOfWork.CountryRepository.GetAll()).ToList();
            ObjectVerifier.VerifyExistence(countries, "Aún no hay paises registrados", countries.Count());
            return countries;
        }
    }
}
