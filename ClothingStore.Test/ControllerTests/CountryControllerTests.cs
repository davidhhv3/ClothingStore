using AutoMapper;
using ClothingStore.Api.Controllers;
using ClothingStore.Api.Responses;
using ClothingStore.Core.CustomEntities;
using ClothingStore.Core.DTOs;
using ClothingStore.Core.Entities;
using ClothingStore.Core.Interfaces;
using ClothingStore.Core.QueryFilters;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System.Diagnostics.Metrics;

namespace ClothingStore.Test.ControllerTests
{
    public class CountryControllerTests
    {
        [Fact]
        public async Task GetCountry_ReturnsCountryDto()
        {

            Country expectedCountry = new Country { Id = 1, Name = "David" };
            CountryDto expectedCountryDto = new CountryDto { Id = 1, Name = "David" };
            var expectedApiResponse = new ApiResponse<CountryDto>(expectedCountryDto);

            var serviceMock = new Mock<IContryService>();
            var mapperMock = new Mock<IMapper>();

            serviceMock.Setup(s => s.GetCountry(1)).ReturnsAsync(expectedCountry);
            mapperMock.Setup(m => m.Map<CountryDto>(expectedCountry)).Returns(expectedCountryDto);

            var controller = new CountryController(serviceMock.Object, mapperMock.Object);

            IActionResult actionResult = await controller.GetCountry(1);
            OkObjectResult okResult = (OkObjectResult)actionResult;
            ApiResponse<CountryDto> returnedApiResponse = Assert.IsType<ApiResponse<CountryDto>>(okResult.Value);

            Assert.Equal(expectedApiResponse.Data, returnedApiResponse.Data);
            Assert.Equal(expectedApiResponse.Meta, returnedApiResponse.Meta);
            Assert.Equal(200, okResult.StatusCode);
            Assert.IsType<ApiResponse<CountryDto>>(returnedApiResponse);
        }

        [Fact]
        public async Task GetCountries_ReturnsCountriesDto()
        {
            Country[] countries = new[]
             {
                new Country{ Id = 1, Name = "Pais 1" },
                new Country{ Id = 2, Name = "Pais 2" },
                new Country{ Id = 3, Name = "Pais 3" },
             };
            IEnumerable<CountryDto> countriesDto = new[]
            {
                new CountryDto{ Id = 1, Name = "Pais 1" },
                new CountryDto{ Id = 2, Name = "Pais 2" },
                new CountryDto{ Id = 3, Name = "Pais 3" },
            };
            Metadata metadata = new Metadata
            {
                TotalCount = 3,
                PageSize = 3,
                CurrentPage = 1,
                TotalPages = 1,
                HasNextPage = false,
                HasPreviousPage = false
            };
            ApiResponse<IEnumerable<CountryDto>> expectedApiResponse = new ApiResponse<IEnumerable<CountryDto>>(countriesDto)
            {
                Meta = metadata
            };

            CountryQueryFilter filters = new CountryQueryFilter
            {
                PageSize = 3,
                PageNumber = 1
            };

            PagedList<Country> pageListServcieResponse = PagedList<Country>.Create(countries, filters.PageNumber, filters.PageSize);
            var serviceMock = new Mock<IContryService>();
            var mapperMock = new Mock<IMapper>();
            serviceMock.Setup(s => s.GetCountries(filters)).Returns(pageListServcieResponse);
            mapperMock.Setup(m => m.Map<IEnumerable<CountryDto>>(pageListServcieResponse)).Returns(countriesDto);


            var controller = new CountryController(serviceMock.Object, mapperMock.Object);

            IActionResult actionResult = controller.GetCountries(filters);
            OkObjectResult okResult = (OkObjectResult)actionResult;

            ApiResponse<IEnumerable<CountryDto>> returnedApiResponse = Assert.IsType<ApiResponse<IEnumerable<CountryDto>>>(okResult.Value);            

            Assert.Equal(expectedApiResponse.Data, returnedApiResponse.Data);
            var properties = typeof(Metadata).GetProperties();
            for (int i = 0; i < properties.Length; i++)
                Assert.Equal(properties[i].GetValue(expectedApiResponse.Meta), properties[i].GetValue(returnedApiResponse.Meta));
            Assert.IsType<ApiResponse<IEnumerable<CountryDto>>>(returnedApiResponse);
        }
        [Fact]
        public async Task CreateCountry_returnCountryDto()
        {
            // Arrange
            Country country = new Country { Name = "Test Country" };
            CountryDto countryDto = new CountryDto { Name = "Test Country" };
            ApiResponse<CountryDto> expectedApiResponse = new ApiResponse<CountryDto>(countryDto);
            Mock<IContryService> mockCountryService = new Mock<IContryService>();
            Mock<IMapper> mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<Country>(countryDto)).Returns(country);
            mapperMock.Setup(m => m.Map<CountryDto>(country)).Returns(countryDto);
            CountryController controller = new CountryController(mockCountryService.Object, mapperMock.Object);

            // Act   
            IActionResult actionResult = await controller.CreateCountry(countryDto);
            OkObjectResult okResult = (OkObjectResult)actionResult;
            ApiResponse<CountryDto> returnedApiResponse = Assert.IsType<ApiResponse<CountryDto>>(okResult.Value);

            // Assert
            mockCountryService.Verify(service => service.InsertCountry(country), Times.Once);
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(returnedApiResponse.Data, expectedApiResponse.Data);
            Assert.Equal(expectedApiResponse.Meta, returnedApiResponse.Meta);
            Assert.IsType<ApiResponse<CountryDto>>(returnedApiResponse);
        }
        [Fact]
        public async Task UpdateCountry_ReturnTrue()
        {
            // Arrange
            var countryDto = new CountryDto { Name = "Test Country" };
            var country = new Country { Id = 1, Name = "Test Country" };
            ApiResponse<bool> expectedApiResponse = new ApiResponse<bool>(true);
            var mockCountryService = new Mock<IContryService>();
            var mapperMock = new Mock<IMapper>();
            mockCountryService.Setup(service => service.UpdateCountry(country)).ReturnsAsync(true);
            mapperMock.Setup(m => m.Map<Country>(countryDto)).Returns(country);
            var controller = new CountryController(mockCountryService.Object, mapperMock.Object);        

            // Act
            var result = await controller.UpdateCountry(1, countryDto);
            var okResult = result as OkObjectResult;
            ApiResponse<bool> returnedApiResponse = Assert.IsType<ApiResponse<bool>>(okResult.Value);

            //Assert
            mockCountryService.Verify(service => service.UpdateCountry(country), Times.Once);
            Assert.NotNull(okResult);
            Assert.Equal(expectedApiResponse.Data, returnedApiResponse.Data);
            Assert.Equal(expectedApiResponse.Meta, returnedApiResponse.Meta);
            Assert.IsType<ApiResponse<bool>>(returnedApiResponse);
        }
        [Fact]
        public async Task DeleteCountry_ReturnTrue()
        {
            // Arrange
            ApiResponse<bool> expectedApiResponse = new ApiResponse<bool>(true);
            Mock<IContryService> mockCountryService = new Mock<IContryService>();
            mockCountryService.Setup(service => service.DeleteCountry(1)).ReturnsAsync(true);
            Mock<IMapper> mapperMock = new Mock<IMapper>();
            CountryController controller = new CountryController(mockCountryService.Object, mapperMock.Object);           

            // Act
            var result = await controller.DeleteCountry(1);
            var okResult = result as OkObjectResult;
            ApiResponse<bool> returnedApiResponse = Assert.IsType<ApiResponse<bool>>(okResult.Value);

            // Assert
            mockCountryService.Verify(service => service.DeleteCountry(1), Times.Once);
            Assert.NotNull(okResult);
            Assert.Equal(expectedApiResponse.Data, returnedApiResponse.Data);
            Assert.Equal(expectedApiResponse.Meta, returnedApiResponse.Meta);
            Assert.IsType<ApiResponse<bool>>(returnedApiResponse);
        }
    }
}
