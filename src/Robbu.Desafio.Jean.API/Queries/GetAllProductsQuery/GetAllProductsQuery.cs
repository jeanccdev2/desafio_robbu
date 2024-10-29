using Robbu.Desafio.Jean.API.Persistence.Entities;
using MediatR;

namespace Robbu.Desafio.Jean.API.Queries.GetAllProductsQuery
{
    public sealed class GetAllProductsQuery : IRequest<IEnumerable<Product>>
    {
        public int? Limit { get; init; }
        public int? Page { get; init; }

        public GetAllProductsQuery(int? limit, int? page)
        {
            Limit = limit;
            Page = page;
        }
    }
}
