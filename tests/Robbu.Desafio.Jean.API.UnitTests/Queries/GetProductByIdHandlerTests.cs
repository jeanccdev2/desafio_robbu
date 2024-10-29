using Moq;
using Robbu.Desafio.Jean.API.Persistence.Entities;
using Robbu.Desafio.Jean.API.Persistence.Repositories;
using Robbu.Desafio.Jean.API.Queries.GetProductByIdQuery;

namespace Robbu.Desafio.Jean.API.UnitTests.Queries
{

    public class GetProductByIdQueryHandlerTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly GetProductByIdQueryHandler _handler;

        public GetProductByIdQueryHandlerTests()
        {
            _mockRepository = new Mock<IProductRepository>();

            _handler = new GetProductByIdQueryHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnsProduct_WhenProductExists()
        {
            // Arrange
            var productId = 1;
            var expectedProduct = new Product { Id = productId, Name = "Test Product" };

            _mockRepository.Setup(repo => repo.GetByIdAsync(productId))
                .ReturnsAsync(expectedProduct);

            var query = new GetProductByIdQuery(productId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedProduct.Id, result.Id);
            Assert.Equal(expectedProduct.Name, result.Name);

            _mockRepository.Verify(repo => repo.GetByIdAsync(productId), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnsNull_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = 2;

            _mockRepository.Setup(repo => repo.GetByIdAsync(productId))
                .ReturnsAsync((Product)null);

            var query = new GetProductByIdQuery(productId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(result);

            _mockRepository.Verify(repo => repo.GetByIdAsync(productId), Times.Once);
        }
    }
}