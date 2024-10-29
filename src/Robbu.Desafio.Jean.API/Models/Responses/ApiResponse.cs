using System.Diagnostics;

namespace Robbu.Desafio.Jean.API.Models.Responses
{
    public class ApiResponse<T>
    {
        public string RequestId { get; init; }
        public string Message { get; init; }
        public T Data { get; init; }

        public ApiResponse(string message, T data)
        {
            RequestId = Activity.Current?.Id ?? new Guid().ToString();
            Message = message;
            Data = data;
        }
    }
}