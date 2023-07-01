using ClothingStore.Core.Entities;
using ClothingStore.Core.Exceptions;
using ClothingStore.Core.Interfaces;
using ClothingStore.Core.Services;
using Moq;

namespace ClothingStore.Test.ServicesTests
{
    public class ClietnServiceTests
    {
        private readonly Mock<IUnitOfWork> mockUnitOfWork;
        private readonly ClientService _clientService;
        public ClietnServiceTests()
        {
            mockUnitOfWork = new Mock<IUnitOfWork>();
            _clientService = new ClientService(mockUnitOfWork.Object);
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
    }
}
