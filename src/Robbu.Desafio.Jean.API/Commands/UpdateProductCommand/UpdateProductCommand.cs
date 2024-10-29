using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Robbu.Desafio.Jean.API.Commands.UpdateProductCommand
{
    public sealed class UpdateProductCommand : BaseProductCommand.BaseProductCommand, IRequest<CommandResult>
    {
        [Required]
        public int Id { get; init; }
    }
}
