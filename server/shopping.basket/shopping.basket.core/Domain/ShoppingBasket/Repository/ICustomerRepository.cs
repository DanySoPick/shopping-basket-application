using shopping.basket.data.Repositories;
using shopping.basket.ShoppingBasket.Models;

namespace shopping.basket.core.Domain.ShoppingBasket.Repository
{
    public interface ICustomerRepository : IGenericRepository
    {
        Task<Customer> GetCustomerByEmailAsync(string email);
    }
}
