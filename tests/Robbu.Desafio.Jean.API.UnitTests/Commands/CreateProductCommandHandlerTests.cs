using Microsoft.EntityFrameworkCore;
using Moq;
using Robbu.Desafio.Jean.API.Commands.CreateProductCommand;
using Robbu.Desafio.Jean.API.Persistence;
using Robbu.Desafio.Jean.API.Persistence.Entities;
using Robbu.Desafio.Jean.API.Persistence.Repositories;
using System.ComponentModel.DataAnnotations;

namespace Robbu.Desafio.Jean.API.UnitTests.Commands
{
    public class CreateProductCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly CreateProductCommandHandler _handler;

        public CreateProductCommandHandlerTests()
        {
            _mockRepository = new Mock<IProductRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _handler = new CreateProductCommandHandler(_mockRepository.Object, _mockUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateProduct_WhenCommandIsValid()
        {
            // Arrange
            var command = new CreateProductCommand
            {
                Name = "Product 1",
                Description = "Description for Product 1",
                Price = 10.0m,
                Date = DateTime.UtcNow
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(command.Name, result.Name);
            Assert.Equal(command.Description, result.Description);
            Assert.Equal(command.Price.Value, result.Price);
            Assert.Equal(command.Date.Value, result.Date);

            _mockRepository.Verify(repo => repo.AddAsync(It.IsAny<Product>()), Times.Once);

            _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenAddFails()
        {
            // Arrange
            var command = new CreateProductCommand
            {
                Name = "Product 2",
                Description = "Description for Product 2",
                Price = 20.0m,
                Date = DateTime.UtcNow
            };

            _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<Product>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));

            _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenCommitFails()
        {
            // Arrange
            var command = new CreateProductCommand
            {
                Name = "Product 3",
                Description = "Description for Product 3",
                Price = 30.0m,
                Date = DateTime.UtcNow
            };

            _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<Product>()))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(uow => uow.CommitAsync())
                .ThrowsAsync(new Exception("Commit error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));

            _mockRepository.Verify(repo => repo.AddAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowsDbUpdateException_WhenCommitFailed()
        {
            // Arrange
            var command = new CreateProductCommand
            {
                Name = null,
                Description = "Description for Product 1",
                Price = 10.0m,
                Date = DateTime.UtcNow
            };

            _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<Product>()))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(uow => uow.CommitAsync())
                .ThrowsAsync(new DbUpdateException("Commit error"));

            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateException>(() => _handler.Handle(command, CancellationToken.None));

            _mockRepository.Verify(repo => repo.AddAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public void CreateProductCommand_ShouldBeInvalid_WhenDateIsNull()
        {
            // Arrange
            var command = new CreateProductCommand
            {
                Name = "Product 1",
                Description = "Description for Product 1",
                Price = 10.0m,
                Date = null
            };

            // Act
            var validationResults = Validate(command);

            // Assert
            Assert.Single(validationResults);
            Assert.Equal("The Date field is required.", validationResults[0].ErrorMessage);
        }

        [Theory]
        [InlineData(null, "Description for Product 1", 10.0, "2024-10-24", new[] { nameof(CreateProductCommand.Name) })]
        [InlineData("Product 1", "Description for Product 1", null, "2024-10-24", new[] { nameof(CreateProductCommand.Price) })]
        [InlineData("Product 1", "Description for Product 1", 10.0, null, new[] { nameof(CreateProductCommand.Date) })]
        public void CreateProductCommand_ShouldBeInvalid_WhenPropertiesAreInvalid(
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
            var command = new CreateProductCommand
            {
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

        [Fact]
        public void CreateProductCommand_ShouldBeValid_WhenAllPropertiesAreSet()
        {
            // Arrange
            var command = new CreateProductCommand
            {
                Name = "Product 1",
                Description = "Description for Product 1",
                Price = 10.0m,
                Date = DateTime.UtcNow
            };

            // Act
            var validationResults = Validate(command);

            // Assert
            Assert.Empty(validationResults);
        }

        private static IList<ValidationResult> Validate(CreateProductCommand command)
        {
            var context = new ValidationContext(command);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(command, context, results, true);
            return results;
        }
    }
}