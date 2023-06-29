using ClothingStore.Core.CustomEntities;
using ClothingStore.Core.Entities;
using ClothingStore.Core.Exceptions;
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
            Country returnCountry = new Country { Id = countryId, Name = "Country 1" };
            mockUnitOfWork.Setup(uow => uow.CountryRepository.GetById(countryId)).ReturnsAsync(returnCountry);
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
        public async Task DeleteCountry_ReturnLaCiudadNoEstáRegistrada()
        {
            // Arrange
            int id = 1;
            mockUnitOfWork.Setup(uow => uow.CountryRepository.GetById(id)).ReturnsAsync((Country?)null);

            // Act and Assert
            var exception = await Assert.ThrowsAsync<BusinessException>(async () =>
            {
                await service.DeleteCountry(id);
            });
            Assert.Equal("La ciudad no está registrada", exception.Message);
            mockUnitOfWork.Verify(uow => uow.CountryRepository.GetById(id), Times.Once);
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
        public async Task GetCountry_ReturnLaCiudadNoEstáRegistrada()
        {
            // Arrange
            int countryId = 1;
            mockUnitOfWork.Setup(uow => uow.CountryRepository.GetById(countryId)).ReturnsAsync((Country?)null);

            // Act and Assert
            var exception = await Assert.ThrowsAsync<BusinessException>(async () =>
            {
                await service.GetCountry(countryId);
            });
            Assert.Equal("La ciudad no está registrada", exception.Message);
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
        public void GetCountries_ReturnAúnNoHayCiudades()
        {
            // Arrange
            CountryQueryFilter filters = new CountryQueryFilter
            {
                PageNumber = 1,
                PageSize = 2
            };
            List<Country> countries = new List<Country>();         
            mockUnitOfWork.Setup(uow => uow.CountryRepository.GetAll()).Returns(countries);

            // Act and Assert
            var exception =  Assert.Throws<BusinessException>(() =>
            {
                service.GetCountries(filters);
            });
            Assert.Equal("Aún no hay ciudades", exception.Message);
            mockUnitOfWork.Verify(uow => uow.CountryRepository.GetAll(), Times.Once);
        }
        [Fact]
        public async Task InsertCountru_ReturnLaCiudadYaEstaRegistrada()
        {
            // Arrange
            Country country = new Country { Id = 1, Name = "Country 1" };
            mockUnitOfWork.Setup(uow => uow.CountryRepository.GetById(country.Id)).ReturnsAsync(country);

            // Act and Assert
            var exception = await Assert.ThrowsAsync<BusinessException>(async () =>
            {
                await service.InsertCountry(country);
            });
            Assert.Equal("La ciudad ya está registrada", exception.Message);
            mockUnitOfWork.Verify(uow => uow.CountryRepository.GetById(country.Id), Times.Once);
        }
        [Fact]
        public async Task InsertCountru_ReturnContentNotAllowed()
        {
            // Arrange
            Country country = new Country { Id = 1, Name = "Pechos" };
            mockUnitOfWork.Setup(uow => uow.CountryRepository.GetById(country.Id)).ReturnsAsync((Country?)null);

            // Act and Assert
            var exception = await Assert.ThrowsAsync<BusinessException>(async () =>
            {
                await service.InsertCountry(country);
            });
            Assert.Equal("Content not allowed", exception.Message);         
        }
        [Fact]
        public async Task UpdateCountry_ReturnTrue()
        {
            // Arrange
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
        [Fact]
        public async Task UpdateCountry_ReturnLaCiudadNoEstáRegistrada()
        {
            // Arrange
            Country country = new Country { Id = 1, Name = "Country 1" };
            mockUnitOfWork.Setup(uow => uow.CountryRepository.GetById(country.Id)).ReturnsAsync((Country?)null);

            // Act and Assert
            var exception = await Assert.ThrowsAsync<BusinessException>(async () =>
            {
                await service.UpdateCountry(country);
            });
            Assert.Equal("La ciudad no está registrada", exception.Message);
            mockUnitOfWork.Verify(uow => uow.CountryRepository.GetById(country.Id), Times.Once);

        }

    }
}
