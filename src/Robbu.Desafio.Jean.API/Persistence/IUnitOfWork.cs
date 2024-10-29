namespace Robbu.Desafio.Jean.API.Persistence
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
    }
}