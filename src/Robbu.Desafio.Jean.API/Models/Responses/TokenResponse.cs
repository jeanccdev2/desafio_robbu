namespace Robbu.Desafio.Jean.API.Models.Responses
{
    public class TokenResponse
    {
        public int ExpirationInHours { get; init; }
        public string Token { get; init; }

        public TokenResponse(int expirationInHours, string token)
        {
            ExpirationInHours = expirationInHours;
            Token = token;
        }
    }
}
