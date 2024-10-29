using Robbu.Desafio.Jean.API.Persistence.Repositories;
using MediatR;

namespace Robbu.Desafio.Jean.API.Queries.GetProductCountQuery
{
    public sealed class GetProductCountQueryHandler : IRequestHandler<GetProductCountQuery, int>
    {
        private readonly IProductRepository _repository;

        public GetProductCountQueryHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(GetProductCountQuery request, CancellationToken cancellationToken)
        {
            var countProducts = await _repository.GetTotalAsync();
            
            return countProducts;
        }
    }
}