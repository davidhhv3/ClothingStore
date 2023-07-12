using ClothingStore.Core.CustomEntities;
using ClothingStore.Core.Entities;
using ClothingStore.Core.Helpers;
using ClothingStore.Core.Interfaces;
using ClothingStore.Core.QueryFilters;
using Microsoft.Extensions.Options;

namespace ClothingStore.Core.Services
{
    public class CountryService : IContryService
    {
        private readonly PaginationOptions _paginationOptions;
        private readonly IUnitOfWork _unitOfWork;

        public CountryService(IUnitOfWork unitOfWork, IOptions<PaginationOptions> options)
        {
            _unitOfWork = unitOfWork;
            _paginationOptions = options.Value;
        }  
        public async Task<bool> DeleteCountry(int id)
        {           
            await CountryServiceHelpers.VerifyCountrieExistence(id, _unitOfWork);
            await _unitOfWork.CountryRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        public async Task<Country?> GetCountry(int Id)
        {
            Country? country = await CountryServiceHelpers.VerifyCountrieExistence(Id, _unitOfWork);
            return country;
        }
        public async Task<PagedList<Country>> GetCountries(CountryQueryFilter filters)
        {
            filters = CountryServiceHelpers.SetValueFilter(filters, _paginationOptions);
            List<Country> countries= await CountryServiceHelpers.VerifyCountriesGetCountries(_unitOfWork);
            PagedList<Country> pagedCountries = PagedList<Country>.Create(countries, filters.PageNumber, filters.PageSize);
            return pagedCountries;
        }

        public async Task<bool> InsertCountry(Country country)
        {             
            await _unitOfWork.CountryRepository.Add(country);
            await _unitOfWork.SaveChangesAsync();    
            return true;
        }
        public async Task<bool> UpdateCountry(Country country)
        {
            Country? existingcountry = await CountryServiceHelpers.VerifyCountrieExistence(country.Id, _unitOfWork);      
            if(existingcountry != null)
            {
                existingcountry.Name = country.Name;
                await _unitOfWork.CountryRepository.Update(existingcountry);
            }           
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }

}
