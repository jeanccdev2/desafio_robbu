using Robbu.Desafio.Jean.API.Models;
using Robbu.Desafio.Jean.API.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Robbu.Desafio.Jean.API.Persistence.DbContexts;

namespace Robbu.Desafio.Jean.API.Persistence.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {

        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Product>> GetAsync(ApiQueries queries)
        {
            return await DbSet
                .Where(p => p.IsDeleted != true)
                .OrderBy(p => p.Id)
                .Skip(queries.PagingPage.Page)
                .Take(queries.PagingPage.Limit)
                .ToListAsync();
        }

        public override async Task<Product?> GetByIdAsync(int id)
        {
            return await DbSet
                .FirstOrDefaultAsync(p => p.Id == id && p.IsDeleted != true);
        }

        public override async Task<int> GetTotalAsync()
        {
            return await DbSet
                .Where(p => p.IsDeleted != true)
                .CountAsync();
        }
    }
}
