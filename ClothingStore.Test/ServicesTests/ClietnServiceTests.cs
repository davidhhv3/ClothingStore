using ClothingStore.Core.CustomEntities;
using ClothingStore.Core.Entities;
using ClothingStore.Core.Exceptions;
using ClothingStore.Core.Interfaces;
using ClothingStore.Core.QueryFilters;
using ClothingStore.Core.Services;
using Microsoft.Extensions.Options;
using Moq;
using System.Diagnostics.Metrics;

namespace ClothingStore.Test.ServicesTests
{
    public class ClietnServiceTests
    {
        private readonly PaginationOptions _paginationOptions;
        private readonly Mock<IUnitOfWork> mockUnitOfWork;
        private readonly Mock<IOptions<PaginationOptions>> mockOptions;
        private readonly ClientService _clientService;
        public ClietnServiceTests()
        {
            mockUnitOfWork = new Mock<IUnitOfWork>();
            mockOptions = new Mock<IOptions<PaginationOptions>>();
            _paginationOptions = new PaginationOptions();
            _clientService = new ClientService(mockUnitOfWork.Object, mockOptions.Object);
            mockOptions.Setup(o => o.Value).Returns(_paginationOptions);

        }
        [Fact]
        public async Task CreateClient_ReturnClient()
        {
            // Arrange
            int clientId = 1;
            Client expectedClient = new Client { Id = 1, Country = 1, Name = "David", LastName = "Hernandez", Age = 30, IdentificationNumber = 1111111111 };
            mockUnitOfWork.Setup(uow => uow.ClientRepository.GetById(clientId)).ReturnsAsync(expectedClient);

            //// Act
            Client result = await _clientService.GetClient(clientId);

            //// Assert
            Assert.NotNull(result);
            Assert.Equal(expectedClient, result);
            mockUnitOfWork.Verify(uow => uow.ClientRepository.GetById(clientId), Times.Once);
        }
        [Fact]
        public async Task CreateClient_ReturnClienteNoRegistrado()
        {
            // Arrange
            int clientId = 1;
            mockUnitOfWork.Setup(uow => uow.ClientRepository.GetById(clientId)).ReturnsAsync((Client?)null);

            // Act and Assert
            var exception = await Assert.ThrowsAsync<BusinessException>(async () =>
            {
                await _clientService.GetClient(clientId);
            });
            Assert.Equal("El cliente no está registrado", exception.Message);
            mockUnitOfWork.Verify(uow => uow.ClientRepository.GetById(clientId), Times.Once);
        }
        [Fact]
        public async Task GetClients_ReturnPagedListClients()
        {
            // Arrange
            ClientQueryFilter filters = new ClientQueryFilter
            {
                PageNumber = 1,
                PageSize = 2
            };
            List<Client> clients = new List<Client>
            {
                new Client { Id = 1, Country = 1, Name = "Client 1", LastName = "LastName 1", Age = 30, IdentificationNumber = 1111111111 },
                new Client { Id = 2, Country = 1, Name = "Client 2", LastName = "LastName 2", Age = 15, IdentificationNumber = 1111111112 },
                new Client { Id = 3, Country = 2, Name = "Client 3", LastName = "LastName 3", Age = 23, IdentificationNumber = 1111111113 },
                new Client { Id = 4, Country = 2, Name = "Client 4", LastName = "LastName 4", Age = 34, IdentificationNumber = 1111111114 },
            };
            mockUnitOfWork.Setup(uow => uow.ClientRepository.GetAll()).ReturnsAsync(clients);

            // Act
            PagedList<Client> result = await _clientService.GetClients(filters);

            // Assert
            Assert.NotNull(result);
            for (int i = 0; i < 2; i++)
                Assert.Equal(clients[i], result[i]);
            mockUnitOfWork.Verify(uow => uow.ClientRepository.GetAll(), Times.Once);
        }
        [Fact]     
        public async Task GetClients_ReturnNoHayClientesRegistrados()
        {
            // Arrange
            ClientQueryFilter filters = new ClientQueryFilter
            {
                PageNumber = 1,
                PageSize = 2
            };
            List<Client> clients = new List<Client>();
            mockUnitOfWork.Setup(uow => uow.ClientRepository.GetAll()).ReturnsAsync(clients);

            // Act and Assert
            var exception = await Assert.ThrowsAsync<BusinessException>(async () =>
            {
                await _clientService.GetClients(filters);
            });
            Assert.Equal("Aún no hay clientes registrados", exception.Message);
            mockUnitOfWork.Verify(uow => uow.ClientRepository.GetAll(), Times.Once);
        }
        [Fact]
        public async Task InsertCountru_ReturnElClienteYaEstaRegistrado()
        {
            // Arrange
            Client client = new Client { Id = 1, Country = 1, Name = "David", LastName = "Hernandez", Age = 30, IdentificationNumber = 1111111111 };
            mockUnitOfWork.Setup(uow => uow.ClientRepository.GetById(client.Id)).ReturnsAsync(client);

            // Act and Assert
            var exception = await Assert.ThrowsAsync<BusinessException>(async () =>
            {
                await _clientService.InsertCLient(client);
            });
            Assert.Equal("El cliente ya está registrado", exception.Message);
            mockUnitOfWork.Verify(uow => uow.ClientRepository.GetById(client.Id), Times.Once);
        }
        [Fact]
        public async Task UpdateClient_ReturnTrue()
        {
            // Arrange
            Client client = new Client { Id = 1, Country = 1, Name = "David", LastName = "Hernandez", Age = 30, IdentificationNumber = 1111111111 };
            mockUnitOfWork.Setup(uow => uow.ClientRepository.GetById(1)).ReturnsAsync(client);
            mockUnitOfWork.Setup(uow => uow.ClientRepository.Update(client)).Verifiable();
            mockUnitOfWork.Setup(uow => uow.SaveChangesAsync()).Verifiable();

            // Act
            bool result = await _clientService.UpdateClient(client);

            // Assert
            Assert.True(result);
            mockUnitOfWork.Verify(uow => uow.ClientRepository.Update(client), Times.Once);
            mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }
        [Fact]
        public async Task UpdateClient_ReturnClienteNoRegistrado()
        {
            // Arrange
            Client client = new Client { Id = 1, Country = 1, Name = "David", LastName = "Hernandez", Age = 30, IdentificationNumber = 1111111111 };          
            mockUnitOfWork.Setup(uow => uow.ClientRepository.GetById(client.Id)).ReturnsAsync((Client?)null);

            // Act and Assert
            var exception = await Assert.ThrowsAsync<BusinessException>(async () =>
            {
                await _clientService.UpdateClient(client);
            });
            Assert.Equal("El cliente no está registrado", exception.Message);
            mockUnitOfWork.Verify(uow => uow.ClientRepository.GetById(client.Id), Times.Once);
        }
        [Fact]
        public async Task DeleteClient_ReturnTrue()
        {
            // Arrange
            int clientId = 1;
            Client returnClient = new Client { Id = 1, Country = 1, Name = "David", LastName = "Hernandez", Age = 30, IdentificationNumber = 1111111111 };
            mockUnitOfWork.Setup(uow => uow.ClientRepository.GetById(clientId)).ReturnsAsync(returnClient);
            mockUnitOfWork.Setup(uow => uow.ClientRepository.Delete(clientId)).Verifiable();
            mockUnitOfWork.Setup(uow => uow.SaveChangesAsync()).Verifiable();

            // Act
            bool result = await _clientService.DeleteClient(clientId);

            // Assert
            Assert.True(result);
            mockUnitOfWork.Verify(uow => uow.ClientRepository.Delete(clientId), Times.Once);
            mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }
        [Fact]
        public async Task DeleteClient_ReturnElClienteNoEstáRegistrada()
        {
            // Arrange
            int id = 1;
            mockUnitOfWork.Setup(uow => uow.ClientRepository.GetById(id)).ReturnsAsync((Client?)null);

            // Act and Assert
            var exception = await Assert.ThrowsAsync<BusinessException>(async () =>
            {
                await _clientService.DeleteClient(id);
            });
            Assert.Equal("El cliente no está registrado", exception.Message);
            mockUnitOfWork.Verify(uow => uow.ClientRepository.GetById(id), Times.Once);
        }
    }
}
