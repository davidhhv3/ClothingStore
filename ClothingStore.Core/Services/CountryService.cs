using ClothingStore.Core.CustomEntities;
using ClothingStore.Core.Entities;
using ClothingStore.Core.Exceptions;
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
            await CountryServiceHelpers.VerifyCityExistence(id,_unitOfWork);
            await _unitOfWork.CountryRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        public async Task<Country> GetCountry(int Id)
        {
            Country country= await CountryServiceHelpers.VerifyCityExistence(Id, _unitOfWork);
            return country;
        }
        public PagedList<Country> GetCountries(CountryQueryFilter filters)
        {
            filters.PageNumber = filters.PageNumber == 0 ? _paginationOptions.DefaultPageNumber : filters.PageNumber;
            filters.PageSize = filters.PageSize == 0 ? _paginationOptions.DefaultPageSize : filters.PageSize;
            List<Country> countries = _unitOfWork.CountryRepository.GetAll().ToList();
            if(countries.Count == 0 || countries == null)
                throw new BusinessException("Aún no hay ciudades");
            PagedList<Country> pagedCountries = PagedList<Country>.Create(countries, filters.PageNumber, filters.PageSize);
            return pagedCountries;
        }

        public async Task InsertCountry(Country country)
        {                  
            Country? existingcountry = await _unitOfWork.CountryRepository.GetById(country.Id);
            if (existingcountry != null)
                throw new BusinessException("La ciudad ya está registrada");
            if (country.Name != null)
                CountryServiceHelpers.CheckForbiddenWords(country.Name);             
            await _unitOfWork.CountryRepository.Add(country);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<bool> UpdateCountry(Country country)
        {
            Country existingcountry = await CountryServiceHelpers.VerifyCityExistence(country.Id, _unitOfWork);
            if(country.Name != null)
              CountryServiceHelpers.CheckForbiddenWords(country.Name);
            existingcountry.Name = country.Name;
            _unitOfWork.CountryRepository.Update(existingcountry);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }

}
