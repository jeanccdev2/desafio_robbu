using Moq;
using Robbu.Desafio.Jean.API.Persistence.Repositories;
using Robbu.Desafio.Jean.API.Queries.GetProductCountQuery;

namespace Robbu.Desafio.Jean.API.UnitTests.Queries
{
    public class GetProductCountQueryHandlerTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly GetProductCountQueryHandler _handler;

        public GetProductCountQueryHandlerTests()
        {
            _mockRepository = new Mock<IProductRepository>();
            _handler = new GetProductCountQueryHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnsCount_WhenProductsExist()
        {
            // Arrange
            var countProducts = 2;

            _mockRepository.Setup(repo => repo.GetTotalAsync())
                .ReturnsAsync(countProducts);

            var query = new GetProductCountQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(2, result);
        }
    }
}