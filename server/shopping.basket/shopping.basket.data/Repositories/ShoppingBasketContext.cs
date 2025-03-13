using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace shopping.basket.data.Repositories
{
    public class ShoppingBasketContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public ShoppingBasketContext(DbContextOptions<ShoppingBasketContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply all configurations dynamically
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 31)));
            }
        }
    }
}