using Microsoft.AspNetCore.Identity;
using Robbu.Desafio.Jean.API.Models.Responses;

namespace Robbu.Desafio.Jean.API.Services
{
    public interface IJwtService
    {
        Task<TokenResponse> GenerateTokenAsync(IdentityUser user);
    }
}
