using System.ComponentModel.DataAnnotations;

namespace Robbu.Desafio.Jean.API.Commands.BaseProductCommand
{
    public abstract class BaseProductCommand
    {
        [Required]
        public string? Name { get; init; }

        public string? Description { get; init; }

        [Required]
        public decimal? Price { get; init; }

        [Required]
        public DateTime? Date { get; init; }
    }
}