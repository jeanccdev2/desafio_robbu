using Moq;
using Robbu.Desafio.Jean.API.Models;
using Robbu.Desafio.Jean.API.Persistence.Entities;
using Robbu.Desafio.Jean.API.Persistence.Repositories;
using Robbu.Desafio.Jean.API.Queries.GetAllProductsQuery;

namespace Robbu.Desafio.Jean.API.UnitTests.Queries
{
    public class GetAllProductsQueryHandlerTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly GetAllProductsQueryHandler _handler;

        public GetAllProductsQueryHandlerTests()
        {
            _mockRepository = new Mock<IProductRepository>();
            _handler = new GetAllProductsQueryHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnsProducts_WhenProductsExist()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1" },
                new Product { Id = 2, Name = "Product 2" }
            };

            _mockRepository.Setup(repo => repo.GetAsync(It.IsAny<ApiQueries>()))
                .ReturnsAsync(products);

            var query = new GetAllProductsQuery(10, 4);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("Product 1", result.First().Name);
            Assert.Equal("Product 2", result.ElementAt(1).Name);
        }

        [Fact]
        public async Task Handle_ShouldReturnsEmptyList_WhenNoProductsExist()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetAsync(It.IsAny<ApiQueries>()))
                .ReturnsAsync(new List<Product>());

            var query = new GetAllProductsQuery(10, 4);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task Handle_ShouldThrowsException_WhenRepositoryThrowsException()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetAsync(It.IsAny<ApiQueries>()))
                .ThrowsAsync(new Exception("Database connection failed"));

            var query = new GetAllProductsQuery(10, 4);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() =>
                _handler.Handle(query, CancellationToken.None));

            Assert.Equal("Database connection failed", exception.Message);
        }
    }
}