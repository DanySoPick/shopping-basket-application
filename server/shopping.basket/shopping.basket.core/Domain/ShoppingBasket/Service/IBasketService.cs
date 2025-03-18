using shopping.basket.core.Domain.ShoppingBasket.Models;

namespace shopping.basket.core.Domain.ShoppingBasket.Service
{
    public interface IBasketService // TODO nice to have ": IDisposable"
    {
        Task<Customer> GetCustomerByEmailAsync(string email);

        //query is sent to the database only when needed, improving efficiency
        Task<IEnumerable<Product>> GetProductsAsync();

        Task<Transaction> InsertTransaction(int customerId, IEnumerable<TransactionItem> transactionItems, IEnumerable<TransactionDiscount> transactionDiscounts);

        Task<IEnumerable<Discount>> GetAvailableDiscountsAsync(DateTime date);
    }
}
