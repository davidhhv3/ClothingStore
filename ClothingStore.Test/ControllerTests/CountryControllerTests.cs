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

namespace ClothingStore.Test.ControllerTests
{
    public class CountryControllerTests
    {
        private readonly Mock<IContryService> mockCountryService;
        private readonly Mock<IMapper> mapperMock;
        private readonly CountryController controller;
        private readonly CountryDto countryDto;
        private readonly Country country;

        public CountryControllerTests()
        {
            mockCountryService = new Mock<IContryService>();
            mapperMock = new Mock<IMapper>();
            controller = new CountryController(mockCountryService.Object, mapperMock.Object);
            countryDto = new CountryDto { Name = "Test Country" };
            country = new Country { Name = "Test Country" };
        }
        [Fact]
        public async Task GetCountry_ReturnsCountryDto()
        {     
            ApiResponse<CountryDto> expectedApiResponse = new ApiResponse<CountryDto>(countryDto);
            mockCountryService.Setup(s => s.GetCountry(1)).ReturnsAsync(country);
            mapperMock.Setup(m => m.Map<CountryDto>(country)).Returns(countryDto);         

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
            mockCountryService.Setup(s => s.GetCountries(filters)).Returns(pageListServcieResponse);
            mapperMock.Setup(m => m.Map<IEnumerable<CountryDto>>(pageListServcieResponse)).Returns(countriesDto);            

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
            ApiResponse<CountryDto> expectedApiResponse = new ApiResponse<CountryDto>(countryDto);         
            mapperMock.Setup(m => m.Map<Country>(countryDto)).Returns(country);
            mapperMock.Setup(m => m.Map<CountryDto>(country)).Returns(countryDto);           

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
            ApiResponse<bool> expectedApiResponse = new ApiResponse<bool>(true);
            mockCountryService.Setup(service => service.UpdateCountry(country)).ReturnsAsync(true);
            mapperMock.Setup(m => m.Map<Country>(countryDto)).Returns(country);  

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
            mockCountryService.Setup(service => service.DeleteCountry(1)).ReturnsAsync(true);                  

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
