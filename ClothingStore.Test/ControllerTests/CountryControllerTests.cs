using AutoMapper;
using ClothingStore.Api.Controllers;
using ClothingStore.Api.Responses;
using ClothingStore.Core.DTOs;
using ClothingStore.Core.Entities;
using ClothingStore.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

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
    }
}
