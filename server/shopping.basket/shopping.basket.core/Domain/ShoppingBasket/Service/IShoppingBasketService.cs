using shopping.basket.ShoppingBasket.Models;

namespace shopping.basket.core.Domain.ShoppingBasket
{
    public interface IShoppingBasketService // TODO nice to have ": IDisposable"
    {
        Task<Customer> GetCustomerByEmail(string email);
    }
}
