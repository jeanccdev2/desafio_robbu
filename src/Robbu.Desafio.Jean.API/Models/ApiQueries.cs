namespace Robbu.Desafio.Jean.API.Models
{
    public class ApiQueries
    {
        public PagingPage? PagingPage { get; init; }

        public ApiQueries(PagingPage? pagingPage)
        {
            PagingPage = pagingPage;
        }
    }
}
