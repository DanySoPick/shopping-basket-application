using Microsoft.EntityFrameworkCore;
using shopping.basket.core.Domain.ShoppingBasket.Repository.Customer;
using shopping.basket.data.Repositories;

namespace shopping.basket.core.Domain.ShoppingBasket.Repository.Customers
{
    public class CustomerRepository : GenericRepository, ICustomerRepository
    {

        public DbSet<Models.Customer> Customers { get; set; }

        public CustomerRepository(DbContextOptions<GenericRepository> options, IConfiguration configuration)
            : base(options, configuration)
        {
        }

        public async Task<Models.Customer?> GetCustomerByEmailAsync(string email)
        {
            return await Customers.FirstOrDefaultAsync(c => c.Email == email);
        }

        //TODO: remove this as its not part of customer....seperate
        public async Task<Models.Customer> InsertItemToBasketAsync(string email)
        {
            return await Customers.FirstOrDefaultAsync(c => c.Email == email);
        }
    }
}
