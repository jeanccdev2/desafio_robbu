using Robbu.Desafio.Jean.API.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Robbu.Desafio.Jean.API.Persistence.DbContexts
{
    public class AppDbContext : DbContext, IDisposable
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.ToTable("products");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsRequired(true)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Price)
                    .IsRequired(true);

                entity.Property(e => e.Date)
                    .IsRequired(true);

                entity.Property(e => e.IsDeleted)
                    .IsRequired(false);
            });
        }
    }
}