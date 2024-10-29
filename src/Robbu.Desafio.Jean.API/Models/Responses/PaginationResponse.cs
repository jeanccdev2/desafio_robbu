namespace Robbu.Desafio.Jean.API.Models.Responses
{
    public class PaginationResponse
    {
        public int TotalRows { get; init; }
        public int TotalPage { get; init; }

        public PaginationResponse(int totalRows, int? limit)
        {
            TotalRows = totalRows;
            var pageSize = limit ?? 25;

            TotalPage = (int)Math.Ceiling((double)totalRows / pageSize);
        }
    }
}