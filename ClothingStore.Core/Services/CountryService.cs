using ClothingStore.Core.Entities;
using ClothingStore.Core.Exceptions;
using ClothingStore.Core.Interfaces;

namespace ClothingStore.Core.Services
{
    public class CountryService : IContryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CountryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> DeleteCountry(int id)
        {
            await _unitOfWork.CountryRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        public async Task<Country> GetCountry(int Id)
        {
            return await _unitOfWork.CountryRepository.GetById(Id);
        }
        public List<Country> GetCountries()
        {
            var countries = _unitOfWork.CountryRepository.GetAll().ToList();
            return countries;
        }
        public async Task InsertCountry(Country country)
        {
            if (country.Name.Contains("sexo"))
                throw new BusinessException("Content not allowed");

            if (country.Name.Contains("pechos"))
                throw new BusinessException("Content not allowed");

            await _unitOfWork.CountryRepository.Add(country);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> UpdateCountry(Country country)
        {
            var existingcountry = await _unitOfWork.CountryRepository.GetById(country.Id);
            existingcountry.Name = country.Name;

            _unitOfWork.CountryRepository.Update(existingcountry);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }

}
