using Robbu.Desafio.Jean.API.Persistence.Entities;
using MediatR;

namespace Robbu.Desafio.Jean.API.Commands.CreateProductCommand
{
    public sealed class CreateProductCommand : BaseProductCommand.BaseProductCommand, IRequest<Product>
    {
    }
}