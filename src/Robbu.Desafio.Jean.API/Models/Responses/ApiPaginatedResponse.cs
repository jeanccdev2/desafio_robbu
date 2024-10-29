namespace Robbu.Desafio.Jean.API.Models.Responses
{
    public class ApiPaginatedResponse<T> : ApiResponse<T>
    {
        public PaginationResponse? Pagination { get; init; }

        public ApiPaginatedResponse(string message, T data, PaginationResponse? pagination) : base(message, data)
        {
            Pagination = pagination ?? null;
        }       
    }
}