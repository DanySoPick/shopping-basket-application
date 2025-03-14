using Microsoft.EntityFrameworkCore;
using shopping.basket.data.Models;
using System.Reflection;
using System.Reflection.Metadata;
using System.Xml.Linq;

namespace shopping.basket.data.Repositories
{
    public class GenericRepository : DbContext, IGenericRepository
    {
        private readonly IConfiguration _configuration;

        public GenericRepository(DbContextOptions<GenericRepository> options, IConfiguration configuration)
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