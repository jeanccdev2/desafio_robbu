using System.ComponentModel.DataAnnotations;

namespace Robbu.Desafio.Jean.API.Models.Requests
{
    public class PostLoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; init; }

        [Required]
        public string Password { get; init; }
    }
}
