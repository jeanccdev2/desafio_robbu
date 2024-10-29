using Robbu.Desafio.Jean.API.Persistence.Entities;
using Robbu.Desafio.Jean.API.Persistence.Repositories;
using MediatR;

namespace Robbu.Desafio.Jean.API.Queries.GetProductByIdQuery
{
    public sealed class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Product>
    {
        private readonly IProductRepository _repository;

        public GetProductByIdQueryHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<Product> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(request.Id);

            return product;
        }
    }
}