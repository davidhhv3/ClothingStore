using ClothingStore.Core.CustomEntities;
using ClothingStore.Core.Entities;
using ClothingStore.Core.Interfaces;
using ClothingStore.Core.QueryFilters;
using ClothingStore.Core.Services;
using Microsoft.Extensions.Options;
using Moq;

namespace ClothingStore.Test.ServicesTests
{
    public class CountryServiceTests
    {
        private readonly PaginationOptions _paginationOptions;
        private readonly Mock<IUnitOfWork> mockUnitOfWork;
        private readonly Mock<IOptions<PaginationOptions>> mockOptions;
        private readonly CountryService service;

        public CountryServiceTests()
        {
            mockUnitOfWork = new Mock<IUnitOfWork>();
            mockOptions = new Mock<IOptions<PaginationOptions>>();
            _paginationOptions = new PaginationOptions();
            service = new CountryService(mockUnitOfWork.Object, mockOptions.Object);
            mockOptions.Setup(o => o.Value).Returns(_paginationOptions);
        }

        [Fact]
        public async Task DeleteCountry_ReturnTrue()
        {
            // Arrange
            int countryId = 1;
            mockUnitOfWork.Setup(uow => uow.CountryRepository.Delete(countryId)).Verifiable();
            mockUnitOfWork.Setup(uow => uow.SaveChangesAsync()).Verifiable();

            // Act
            bool result = await service.DeleteCountry(countryId);

            // Assert
            Assert.True(result);
            mockUnitOfWork.Verify(uow => uow.CountryRepository.Delete(countryId), Times.Once);
            mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }
        [Fact]
        public async Task GetCountry_ReturnCountry()
        {
            // Arrange
            int countryId = 1;
            Country expectedCountry = new Country { Id = countryId, Name = "Country 1" };
            mockUnitOfWork.Setup(uow => uow.CountryRepository.GetById(countryId)).ReturnsAsync(expectedCountry);

            // Act
            Country result = await service.GetCountry(countryId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedCountry, result);           
            mockUnitOfWork.Verify(uow => uow.CountryRepository.GetById(countryId), Times.Once);
        }
 
        [Fact]
        public void GetCountries_ReturnPagedListCountries()
        {
            // Arrange
            CountryQueryFilter filters = new CountryQueryFilter
            {
                PageNumber = 1,
                PageSize = 2
            };
            List<Country> countries = new List<Country>
            {
                new Country { Id = 1, Name = "Country 1" },
                new Country { Id = 2, Name = "Country 2" },
                new Country { Id = 3, Name = "Country 3" },
                new Country { Id = 4, Name = "Country 4" },
            };
            mockUnitOfWork.Setup(uow => uow.CountryRepository.GetAll()).Returns(countries);

            // Act
            PagedList<Country> result = service.GetCountries(filters);

            // Assert
            Assert.NotNull(result);
            for (int i = 0; i < 2; i++)
                Assert.Equal(countries[i], result[i]);
            mockUnitOfWork.Verify(uow => uow.CountryRepository.GetAll(), Times.Once);
        }
        [Fact]
        public async Task UpdateCountry_ReturnTrue()
        {
            Country country = new Country { Id = 1, Name = "Country 1" };
            mockUnitOfWork.Setup(uow => uow.CountryRepository.GetById(1)).ReturnsAsync(country);
            mockUnitOfWork.Setup(uow => uow.CountryRepository.Update(country)).Verifiable();
            mockUnitOfWork.Setup(uow => uow.SaveChangesAsync()).Verifiable();

            // Act
            bool result = await service.UpdateCountry(country);

            // Assert
            Assert.True(result);
            mockUnitOfWork.Verify(uow => uow.CountryRepository.Update(country), Times.Once);
            mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }
    }
}
