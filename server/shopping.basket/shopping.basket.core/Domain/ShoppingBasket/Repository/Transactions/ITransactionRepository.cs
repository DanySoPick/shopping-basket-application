using shopping.basket.core.Domain.ShoppingBasket.Models;
using shopping.basket.data.Repositories;

namespace shopping.basket.core.Domain.ShoppingBasket.Repository.Transactions
{
    public interface ITransactionRepository : IGenericRepository
    {
        Task<Transaction> GetTransactionByIdAsync(int id);

        Task<IEnumerable<TransactionDiscount>> GetDiscountedTransactionsAsync(int id);

        Task<Transaction> InsertTransactionAsync(Transaction transaction);

    }
}
