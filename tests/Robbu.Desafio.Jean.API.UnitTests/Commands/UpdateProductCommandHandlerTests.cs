using Moq;
using Robbu.Desafio.Jean.API.Commands.UpdateProductCommand;
using Robbu.Desafio.Jean.API.Persistence;
using Robbu.Desafio.Jean.API.Persistence.Entities;
using Robbu.Desafio.Jean.API.Persistence.Repositories;
using System.ComponentModel.DataAnnotations;

namespace Robbu.Desafio.Jean.API.UnitTests.Commands
{
    public class UpdateProductCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly UpdateProductCommandHandler _handler;

        public UpdateProductCommandHandlerTests()
        {
            _mockRepository = new Mock<IProductRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _handler = new UpdateProductCommandHandler(_mockRepository.Object, _mockUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_ShouldUpdateProduct_WhenProductExists()
        {
            // Arrange
            var command = new UpdateProductCommand
            {
                Id = 1,
                Name = "Updated Product",
                Description = "Updated Description",
                Price = 15.0m,
                Date = DateTime.UtcNow
            };

            var product = new Product
            {
                Id = command.Id,
                Name = "Original Product",
                Description = "Original Description",
                Price = 10.0m,
                Date = DateTime.UtcNow.AddDays(-1)
            };

            _mockRepository.Setup(repo => repo.GetByIdAsync(command.Id))
                           .ReturnsAsync(product);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccessful);
            Assert.Equal(command.Name, product.Name);
            Assert.Equal(command.Description, product.Description);
            Assert.Equal(command.Price.Value, product.Price);
            Assert.Equal(command.Date.Value, product.Date);

            _mockRepository.Verify(repo => repo.Update(It.IsAny<Product>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var command = new UpdateProductCommand
            {
                Id = 1,
                Name = "Non-existent Product",
                Description = "Non-existent Description",
                Price = 20.0m,
                Date = DateTime.UtcNow
            };

            _mockRepository.Setup(repo => repo.GetByIdAsync(command.Id))
                           .ReturnsAsync((Product)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccessful);
            Assert.True(result.IsNotFound);

            _mockRepository.Verify(repo => repo.Update(It.IsAny<Product>()), Times.Never);
            _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenUpdateFails()
        {
            // Arrange
            var command = new UpdateProductCommand
            {
                Id = 1,
                Name = "Product Update",
                Description = "Description for Product Update",
                Price = 25.0m,
                Date = DateTime.UtcNow
            };

            var product = new Product
            {
                Id = command.Id,
                Name = "Original Product",
                Description = "Original Description",
                Price = 20.0m,
                Date = DateTime.UtcNow.AddDays(-2)
            };

            _mockRepository.Setup(repo => repo.GetByIdAsync(command.Id))
                           .ReturnsAsync(product);

            _mockRepository.Setup(repo => repo.Update(It.IsAny<Product>()))
                           .Throws(new Exception("Update error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));

            _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenCommitFails()
        {
            // Arrange
            var command = new UpdateProductCommand
            {
                Id = 1,
                Name = "Product Update",
                Description = "Description for Product Update",
                Price = 30.0m,
                Date = DateTime.UtcNow
            };

            var product = new Product
            {
                Id = command.Id,
                Name = "Original Product",
                Description = "Original Description",
                Price = 20.0m,
                Date = DateTime.UtcNow.AddDays(-3)
            };

            _mockRepository.Setup(repo => repo.GetByIdAsync(command.Id))
                           .ReturnsAsync(product);

            _mockUnitOfWork.Setup(uow => uow.CommitAsync())
                           .Throws(new Exception("Commit error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));

            _mockRepository.Verify(repo => repo.Update(It.IsAny<Product>()), Times.Once);
        }

        [Theory]
        [InlineData(1, null, "Descrição de Produto 1", 10.0, "2024-10-24", new[] { nameof(UpdateProductCommand.Name) })]
        [InlineData(1, "Produto 1", "Descrição de Produto 1", null, "2024-10-24", new[] { nameof(UpdateProductCommand.Price) })]
        [InlineData(1, "Produto 1", "Descrição de Produto 1", 10.0, null, new[] { nameof(UpdateProductCommand.Date) })]
        public void UpdateProductCommand_ShouldBeInvalid_WhenPropertiesAreInvalid(
            int id,
            string name,
            string description,
            double? priceAsDouble,
            string? dateString,
            string[] memberNames)
        {
            // Convertendo double para decimal
            decimal? price = priceAsDouble.HasValue ? Convert.ToDecimal(priceAsDouble.Value) : null;

            // Convertendo a string para DateTime
            DateTime? date = string.IsNullOrEmpty(dateString) ? null : DateTime.Parse(dateString);

            // Arrange
            var command = new UpdateProductCommand
            {
                Id = id,
                Name = name,
                Description = description,
                Price = price,
                Date = date
            };

            // Act
            var validationResults = Validate(command);

            // Assert
            Assert.Equal(memberNames.Length, validationResults.Count);
            foreach (var memberName in memberNames)
            {
                Assert.Contains(validationResults, v => v.MemberNames.Contains(memberName));
            }
        }

        private static IList<ValidationResult> Validate(UpdateProductCommand command)
        {
            var context = new ValidationContext(command);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(command, context, results, true);
            return results;
        }
    }
}
