using Robbu.Desafio.Jean.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Robbu.Desafio.Jean.API.Persistence.Repositories
{
    public interface IRepository<T> where T : class
    {
        DbSet<T> DbSet { get; }

        Task<T?> GetByIdAsync(int id);

        Task<IEnumerable<T>> GetAsync(ApiQueries queries);

        Task<int> GetTotalAsync();

        Task AddAsync(T entity);

        void Update(T entity);

        void Delete(T entity);
    }
}