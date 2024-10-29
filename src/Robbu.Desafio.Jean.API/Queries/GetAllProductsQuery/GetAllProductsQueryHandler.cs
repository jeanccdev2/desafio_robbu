using Robbu.Desafio.Jean.API.Models;
using Robbu.Desafio.Jean.API.Persistence.Entities;
using Robbu.Desafio.Jean.API.Persistence.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Robbu.Desafio.Jean.API.Queries.GetAllProductsQuery
{
    public sealed class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<Product>>
    {
        private readonly IProductRepository _repository;

        public GetAllProductsQueryHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var pagingPage = new PagingPage(request.Limit, request.Page);
            var apiQueries = new ApiQueries(pagingPage);
            var products = await _repository.GetAsync(apiQueries);

            return products;
        }
    }
}
