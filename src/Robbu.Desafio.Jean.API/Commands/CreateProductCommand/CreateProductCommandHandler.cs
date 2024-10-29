using Robbu.Desafio.Jean.API.Persistence.Entities;
using Robbu.Desafio.Jean.API.Persistence;
using Robbu.Desafio.Jean.API.Persistence.Repositories;
using MediatR;

namespace Robbu.Desafio.Jean.API.Commands.CreateProductCommand
{
    public sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Product>
    {
        private readonly IProductRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductCommandHandler(IProductRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Product> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product()
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price.Value,
                Date = request.Date.Value
            };

            await _repository.AddAsync(product);
            await _unitOfWork.CommitAsync();
            
            return product;
        }
    }
}