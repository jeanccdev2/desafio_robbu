using System.ComponentModel.DataAnnotations;
using Robbu.Desafio.Jean.API.Persistence.Entities;
using MediatR;

namespace Robbu.Desafio.Jean.API.Queries.GetProductByIdQuery
{
    public sealed class GetProductByIdQuery : IRequest<Product>
    {
        [Required]
        public int Id { get; init; }

        public GetProductByIdQuery(int id)
        {
            Id = id;
        }
    }
}