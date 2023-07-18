using AutoMapper;
using ClothingStore.Api.Controllers;
using ClothingStore.Api.Responses;
using ClothingStore.Core.CustomEntities;
using ClothingStore.Core.DTOs;
using ClothingStore.Core.Entities;
using ClothingStore.Core.Interfaces;
using ClothingStore.Core.QueryFilters;
using ClothingStore.Test.Helpers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ClothingStore.Test.ControllerTests
{
    public class ClientControllerTests
    {
        private readonly Mock<IClientService> mockClientService;
        private readonly Mock<IMapper> mapperMock;
        private readonly ClientController controller;
        private readonly ClientDto clientDto;
        private readonly Client client;

        public ClientControllerTests()
        {
            mockClientService = new Mock<IClientService>();
            mapperMock = new Mock<IMapper>();
            controller = new ClientController(mockClientService.Object, mapperMock.Object);
            clientDto = new ClientDto {Name = "David",LastName= "Hernandez",Age = 30,Country = 1};
            client = new Client { Name = "David", LastName = "Hernandez", Age = 30, Country = 1,IdentificationNumber=12345678 };
        }
        [Fact]
        public async Task GetClient_ReturnsClientDto()
        {
            ApiResponse<ClientDto> expectedApiResponse = new ApiResponse<ClientDto>(clientDto);
            mockClientService.Setup(s => s.GetClient(1)).ReturnsAsync(client);
            mapperMock.Setup(m => m.Map<ClientDto>(client)).Returns(clientDto);

            IActionResult actionResult = await controller.GetClient(1);
            OkObjectResult okResult = (OkObjectResult)actionResult;
            ApiResponse<ClientDto> returnedApiResponse = Assert.IsType<ApiResponse<ClientDto>>(okResult.Value);

            mockClientService.Verify(service => service.GetClient(1), Times.Once);
            ControllerTestsHelpers.checkResponseApi(okResult, returnedApiResponse, expectedApiResponse);
        }
        [Fact]
        public async Task GetClients_ReturnsClientsDto()
        {
            Client[] clients = new[]
             {
                new Client{ Id = 1,  Name = "Client 1", LastName = "LastName 1", Age = 30, Country = 1,IdentificationNumber = 1234567890 },
                new Client{ Id = 2,  Name = "Client 2", LastName = "LastName 2", Age = 23, Country = 1,IdentificationNumber = 1234567891 },
                new Client{ Id = 3,  Name = "Client 3", LastName = "LastName 3", Age = 42, Country = 2,IdentificationNumber = 1234567892 },
             };
            IEnumerable<ClientDto> clientsDto = new[]
            {
                new ClientDto{ Id = 1,  Name = "Client 1", LastName = "LastName 1", Age = 30, Country = 1},
                new ClientDto{ Id = 2,  Name = "Client 2", LastName = "LastName 2", Age = 23, Country = 1},
                new ClientDto{ Id = 3,  Name = "Client 3", LastName = "LastName 3", Age = 42, Country = 2},
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
            ApiResponse<IEnumerable<ClientDto>> expectedApiResponse = new ApiResponse<IEnumerable<ClientDto>>(clientsDto)
            {
                Meta = metadata
            };

            ClientQueryFilter filters = new ClientQueryFilter
            {
                PageSize = 3,
                PageNumber = 1
            };

            PagedList<Client> pageListServcieResponse = PagedList<Client>.Create(clients, filters.PageNumber, filters.PageSize);
            mockClientService.Setup(s => s.GetClients(filters)).ReturnsAsync(pageListServcieResponse);
            mapperMock.Setup(m => m.Map<IEnumerable<ClientDto>>(pageListServcieResponse)).Returns(clientsDto);

            IActionResult actionResult = await controller.GetClients(filters);
            OkObjectResult okResult = (OkObjectResult)actionResult;

            ApiResponse<IEnumerable<ClientDto>> returnedApiResponse = Assert.IsType<ApiResponse<IEnumerable<ClientDto>>>(okResult.Value);

            Assert.Equal(expectedApiResponse.Data, returnedApiResponse.Data);
            var properties = typeof(Metadata).GetProperties();
            for (int i = 0; i < properties.Length; i++)
                Assert.Equal(properties[i].GetValue(expectedApiResponse.Meta), properties[i].GetValue(returnedApiResponse.Meta));
            Assert.IsType<ApiResponse<IEnumerable<ClientDto>>>(returnedApiResponse);
        }
        [Fact]
        public async Task GetClientsByCountry_ReturnsClientsDto()
        {
            Client[] clientsByCountry = new[]
            {
                new Client{ Id = 1,  Name = "Client 1", LastName = "LastName 1", Age = 30, Country = 1,IdentificationNumber = 1234567890 },
                new Client{ Id = 2,  Name = "Client 2", LastName = "LastName 2", Age = 23, Country = 1,IdentificationNumber = 1234567891 },
             };
            IEnumerable<ClientDto> clientsByCountryDto = new[]
            {
                new ClientDto{ Id = 1,  Name = "Client 1", LastName = "LastName 1", Age = 30, Country = 1},
                new ClientDto{ Id = 2,  Name = "Client 2", LastName = "LastName 2", Age = 23, Country = 1},         
            };
            Metadata metadata = new Metadata
            {
                TotalCount = 2,
                PageSize = 3,
                CurrentPage = 1,
                TotalPages = 1,
                HasNextPage = false,
                HasPreviousPage = false
            };
            ApiResponse<IEnumerable<ClientDto>> expectedApiResponse = new ApiResponse<IEnumerable<ClientDto>>(clientsByCountryDto)
            {
                Meta = metadata
            };

            ClientQueryFilter filters = new ClientQueryFilter
            {
                PageSize = 3,
                PageNumber = 1
            };

            PagedList<Client> pageListServcieResponse = PagedList<Client>.Create(clientsByCountry, filters.PageNumber, filters.PageSize);
            mockClientService.Setup(s => s.GetClientsByCountry(filters,1)).ReturnsAsync(pageListServcieResponse);          
            mapperMock.Setup(m => m.Map<IEnumerable<ClientDto>>(pageListServcieResponse)).Returns(clientsByCountryDto);
            IActionResult actionResult = await controller.GetClientsBycountry(filters, 1);                                                       
            OkObjectResult okResult = (OkObjectResult)actionResult;
            ApiResponse<IEnumerable<ClientDto>> returnedApiResponse = Assert.IsType<ApiResponse<IEnumerable<ClientDto>>>(okResult.Value);

            Assert.Equal(expectedApiResponse.Data, returnedApiResponse.Data);            
            var properties = typeof(Metadata).GetProperties();  
            for (int i = 0; i < properties.Length; i++)
                Assert.Equal(properties[i].GetValue(expectedApiResponse.Meta), properties[i].GetValue(returnedApiResponse.Meta));
            Assert.IsType<ApiResponse<IEnumerable<ClientDto>>>(returnedApiResponse);
        }
        [Fact]
        public async Task CreateClient_returnClientDto()
        {
            // Arrange 
            Client client = new Client { Id = 1, Name = "Client 1", LastName = "LastName 1", Age = 30, Country = 1, IdentificationNumber = 1234567890};
            ClientDto clientDto = new ClientDto{ Id = 1,  Name = "Client 1", LastName = "LastName 1", Age = 30, Country = 1};           
            ApiResponse<ClientDto> expectedApiResponse = new ApiResponse<ClientDto>(clientDto);
            mapperMock.Setup(m => m.Map<Client>(clientDto)).Returns(client);
            mapperMock.Setup(m => m.Map<ClientDto>(client)).Returns(clientDto);

            // Act   
            IActionResult actionResult = await controller.CreateClient(client);
            OkObjectResult okResult = (OkObjectResult)actionResult;
            ApiResponse<ClientDto> returnedApiResponse = Assert.IsType<ApiResponse<ClientDto>>(okResult.Value);
            // Assert
            mockClientService.Verify(service => service.InsertCLient(client), Times.Once);
            ControllerTestsHelpers.checkResponseApi(okResult, returnedApiResponse, expectedApiResponse);
        }
        [Fact]
        public async Task UpdateClient_ReturnTrue()
        {
            // Arrange      
            Client client = new Client { Id = 1, Name = "Client 1", LastName = "LastName 1", Age = 30, Country = 1, IdentificationNumber = 1234567890 };
            ClientDto clientDto = new ClientDto { Id = 1, Name = "Client 1", LastName = "LastName 1", Age = 30, Country = 1 };
            ApiResponse<bool> expectedApiResponse = new ApiResponse<bool>(true);
            mockClientService.Setup(service => service.UpdateClient(client)).ReturnsAsync(true);
            mapperMock.Setup(m => m.Map<Client>(clientDto)).Returns(client);          

            // Act
            var result = await controller.UpdateClient(1, clientDto);
            OkObjectResult okResult = result as OkObjectResult ?? throw new ArgumentNullException(nameof(result));
            ApiResponse<bool> returnedApiResponse = Assert.IsType<ApiResponse<bool>>(okResult.Value);

            //Assert
            mockClientService.Verify(service => service.UpdateClient(client), Times.Once);
            ControllerTestsHelpers.checkResponseApi(okResult, returnedApiResponse, expectedApiResponse);
        }
        [Fact]
        public async Task DeleteClient_ReturnTrue()
        {
            // Arrange
            ApiResponse<bool> expectedApiResponse = new ApiResponse<bool>(true);
            mockClientService.Setup(service => service.DeleteClient(1)).ReturnsAsync(true);

            // Act
            var result = await controller.DeleteClient(1);
            OkObjectResult okResult = result as OkObjectResult ?? throw new ArgumentNullException(nameof(result));
            ApiResponse<bool> returnedApiResponse = Assert.IsType<ApiResponse<bool>>(okResult.Value);

            // Assert
            mockClientService.Verify(service => service.DeleteClient(1), Times.Once);
            ControllerTestsHelpers.checkResponseApi(okResult, returnedApiResponse, expectedApiResponse);
        }
    }
}
