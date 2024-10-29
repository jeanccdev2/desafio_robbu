using Moq;
using Robbu.Desafio.Jean.API.Commands.DeleteProductCommand;
using Robbu.Desafio.Jean.API.Persistence;
using Robbu.Desafio.Jean.API.Persistence.Entities;
using Robbu.Desafio.Jean.API.Persistence.Repositories;

namespace Robbu.Desafio.Jean.API.UnitTests.Commands
{
    public class DeleteProductCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly DeleteProductCommandHandler _handler;

        public DeleteProductCommandHandlerTests()
        {
            _mockRepository = new Mock<IProductRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _handler = new DeleteProductCommandHandler(_mockRepository.Object, _mockUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_ProductExists_ShouldReturnSuccess()
        {
            // Arrange
            var command = new DeleteProductCommand(1);
            var product = new Product
            {
                Id = 1,
                Name = "Produto teste",
                Description = "Produto teste",
                Price = 1,
                Date = DateTime.Now,
                IsDeleted = false
            };

            _mockRepository.Setup(repo => repo.GetByIdAsync(command.Id))
                .ReturnsAsync(product);
            _mockUnitOfWork.Setup(uow => uow.CommitAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccessful);
            _mockRepository.Verify(repo => repo.Update(product), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);
            Assert.True(product.IsDeleted);
        }

        [Fact]
        public async Task Handle_ProductDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var command = new DeleteProductCommand(1);

            _mockRepository.Setup(repo => repo.GetByIdAsync(command.Id))
                .ReturnsAsync((Product)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.True(result.IsNotFound);
            _mockRepository.Verify(repo => repo.Update(It.IsAny<Product>()), Times.Never);
            _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Never);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task Handle_ProductExistsWithDifferentIds_ShouldReturnSuccess(int productId)
        {
            // Arrange
            var command = new DeleteProductCommand(productId);
            var product = new Product
            {
                Id = productId,
                Name = "Produto teste",
                Description = "Produto teste",
                Price = 1,
                Date = DateTime.Now,
                IsDeleted = false
            };

            _mockRepository.Setup(repo => repo.GetByIdAsync(productId))
                .ReturnsAsync(product);
            _mockUnitOfWork.Setup(uow => uow.CommitAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccessful);
            _mockRepository.Verify(repo => repo.Update(product), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);
            Assert.True(product.IsDeleted);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Handle_InvalidProductId_ShouldReturnNotFound(int productId)
        {
            // Arrange
            var command = new DeleteProductCommand(productId);

            _mockRepository.Setup(repo => repo.GetByIdAsync(productId))
                .ReturnsAsync((Product)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.True(result.IsNotFound);
            _mockRepository.Verify(repo => repo.Update(It.IsAny<Product>()), Times.Never);
            _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Never);
        }
    }
}