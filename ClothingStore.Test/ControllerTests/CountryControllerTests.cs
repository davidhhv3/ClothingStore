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
            Assert.Equal(200, okResult.StatusCode);
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
        }
    }
}
