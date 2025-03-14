using Microsoft.EntityFrameworkCore;
using shopping.basket.data.Repositories;
using shopping.basket.ShoppingBasket.Models;

namespace shopping.basket.core.Domain.ShoppingBasket.Repository
{
    public class CustomerRepository : GenericRepository, ICustomerRepository
    {

        public DbSet<Customer> Customers { get; set; }

        public CustomerRepository(DbContextOptions<GenericRepository> options, IConfiguration configuration)
            : base(options, configuration)
        {
        }

        public async Task<Customer?> GetCustomerByEmailAsync(string email)
        {
            return await Customers.FirstOrDefaultAsync(c => c.Email == email);
        }
    }
}
