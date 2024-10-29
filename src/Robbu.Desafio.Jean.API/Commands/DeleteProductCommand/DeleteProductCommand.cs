using MediatR;
using System;

namespace Robbu.Desafio.Jean.API.Commands.DeleteProductCommand
{
    public sealed class DeleteProductCommand : IRequest<CommandResult>
    {
        public int Id { get; init; }

        public DeleteProductCommand(int id)
        {
            Id = id;
        }
    }
}
