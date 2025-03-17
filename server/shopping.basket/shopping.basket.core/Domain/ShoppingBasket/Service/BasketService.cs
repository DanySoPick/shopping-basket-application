using shopping.basket.core.Domain.ShoppingBasket.Models;
using shopping.basket.core.Domain.ShoppingBasket.Repository.Customer;
using shopping.basket.core.Domain.ShoppingBasket.Repository.Products;
using shopping.basket.core.Domain.ShoppingBasket.Repository.Transactions;

namespace shopping.basket.core.Domain.ShoppingBasket.Service
{
    public class BasketService : IBasketService
    {
        private ILogger<BasketService> _logger;
        private ICustomerRepository _customerRepository;
        private IProductRepository _productRepository;
        private ITransactionRepository _transactionRepository;

        public BasketService(ILogger<BasketService> logger, ICustomerRepository customerRepository, IProductRepository productRepository, ITransactionRepository transactionRepository
            ) {
            _logger = logger;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<Customer> GetCustomerByEmailAsync(string email)
        {
            try
            {
                return await _customerRepository.GetCustomerByEmailAsync(email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public  async Task<IEnumerable<Product>> GetProductsAsync()
        {
            try
            {
                return await _productRepository.GetProductsAsync();
            }
            catch (Exception ex)
            {
                // This types of errors better to be handled internally
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Transaction> InsertTransaction(int customerId, IEnumerable<TransactionItem> transactionItems, IEnumerable<TransactionDiscount> transactionDiscounts)
        {
            try
            {
                // Create a new transaction
                var transaction = new Transaction
                {
                    CustomerId = customerId,
                    TransactionDate = DateTime.UtcNow,
                    TransactionItems = new List<TransactionItem>()
                };

                // Populate transaction items
                foreach (var selectedProduct in transactionItems)
                {
                    var product = await _productRepository.FindProductAsync(selectedProduct.ProductId);
                    if (product == null)
                    {
                        throw new Exception($"Product with ID {selectedProduct.ProductId} not found.");
                    }

                    var transactionItem = new TransactionItem
                    {
                        ProductId = product.Id,
                        Quantity = selectedProduct.Quantity,
                        Price = product.Price
                    };

                    transaction.TransactionItems.Add(transactionItem);
                }

                // Populate transaction discounts
                foreach (var discount in transactionDiscounts)
                {
                    var transactionDiscount = new TransactionDiscount
                    {
                        DiscountId = discount.DiscountId
                    };

                    transaction.TransactionDiscounts.Add(transactionDiscount);
                }

                // Save the transaction to the database (assuming you have a method for this)
                await _transactionRepository.InsertTransactionAsync(transaction);

                return transaction;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception("An error occurred while inserting the transaction.", ex);
            }
        }
    }
}
