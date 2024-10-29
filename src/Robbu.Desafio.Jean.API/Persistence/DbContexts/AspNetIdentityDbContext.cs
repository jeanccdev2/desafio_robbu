using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Robbu.Desafio.Jean.API.Persistence.DbContexts
{
    public class AspNetIdentityDbContext : IdentityDbContext<IdentityUser>
    {
        public AspNetIdentityDbContext(DbContextOptions<AspNetIdentityDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Verifique se já não está configurado (útil se estiver usando AddDbContext em algum lugar)
            //if (!optionsBuilder.IsConfigured)
            //{
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=vicris;Username=postgres;Password=Hayom0607!;");
            //}
        }
    }
}
