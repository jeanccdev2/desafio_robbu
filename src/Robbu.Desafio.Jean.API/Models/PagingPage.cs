namespace Robbu.Desafio.Jean.API.Models
{
    public class PagingPage
    {
        public int Limit { get; init; }
        public int Page { get; init; }

        public PagingPage(int? limit, int? page)
        {
            Limit = limit ?? 25;
            Page = ((page ?? 1) - 1) * Limit;
        }
    }
}
