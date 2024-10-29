using Robbu.Desafio.Jean.API.Models;
using Microsoft.EntityFrameworkCore;
using Robbu.Desafio.Jean.API.Persistence.DbContexts;

namespace Robbu.Desafio.Jean.API.Persistence.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbSet<T> _dbSet;

        public DbSet<T> DbSet => _dbSet;

        public Repository(AppDbContext context)
        {
            _dbSet = context.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAsync(ApiQueries queries)
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<int> GetTotalAsync()
        {
            return await _dbSet.CountAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }
    }
}