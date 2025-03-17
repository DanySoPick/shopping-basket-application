using Microsoft.EntityFrameworkCore;
using shopping.basket.core.Domain.ShoppingBasket.Models;
using shopping.basket.data.Repositories;

namespace shopping.basket.core.Domain.ShoppingBasket.Repository.Transactions
{
    public class TransactionRepository : GenericRepository, ITransactionRepository
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionDiscount> Discounts { get; set; }
        public DbSet<TransactionItem> TransactionItems { get; set; }
        public DbSet<Models.Customer> Customers { get; set; }

        private ILogger<TransactionRepository> _logger;

        public TransactionRepository(DbContextOptions<GenericRepository> options, IConfiguration configuration, ILogger<TransactionRepository>  logger)
            : base(options, configuration)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<TransactionDiscount>> GetDiscountedTransactionsAsync(int id)
        {
            return await Discounts
            .FromSqlRaw("CALL get_discounted_transactions({0})", id)
            .ToListAsync();
        }

        public async Task<Transaction> GetTransactionByIdAsync(int id)
        {
            throw new NotImplementedException("InsertNewProductAsync method is not implemented yet.");
        }

        public async Task<Transaction> InsertTransactionAsync(Transaction transaction)
        {
            try
            {
                // Add the transaction to the Transactions DbSet
                await Transactions.AddAsync(transaction);

                // Save changes to the database
                await SaveChangesAsync();

                return transaction;
            }
            catch (Exception ex)
            {
                // Log the error and rethrow the exception
                _logger.LogError(ex.Message);
                throw new Exception("An error occurred while inserting the transaction.", ex);
            }
        }

    }
}
