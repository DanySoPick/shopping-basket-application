using shopping.basket.data.Repositories;

namespace shopping.basket.core.Domain.ShoppingBasket.Repository.Customer
{
    public interface ICustomerRepository : IGenericRepository
    {
        Task<Models.Customer> GetCustomerByEmailAsync(string email);

        Task<Models.Customer> InsertItemToBasketAsync(string email);
    }
}
