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
            )
        {
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

        public async Task<IEnumerable<Product>> GetProductsAsync()
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

        public async Task<IEnumerable<Discount>> GetAvailableDiscountsAsync(DateTime date)
        {
            try
            {
                return await _productRepository.GetAvailableDiscountsAsync(date);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Transaction>> CalculateCheckOutAsync(IEnumerable<TransactionItem> transactionItems, IEnumerable<TransactionDiscount> transactionDiscounts)
        {
            try
            {
                // Apply multi-buy discounts
                ApplyMultiBuyDiscount(transactionItems, transactionDiscounts);

                // Apply direct discounts
                ApplyDirectDiscount(transactionItems, transactionDiscounts);

                // Calculate total cost correctly
                decimal totalCost = transactionItems.Sum(item => item.TotalPrice);

                // Create a new transaction
                var transaction = new Transaction();

                foreach (var discount in transactionDiscounts)
                {
                    transaction.TransactionDiscounts.Add(discount);
                }
                foreach (var item in transactionItems)
                {
                    transaction.TransactionItems.Add(item);
                }

                return new List<Transaction> { transaction };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in checkout calculation");
                throw;
            }
        }

        private void ApplyDirectDiscount(IEnumerable<TransactionItem> transactionItems, IEnumerable<TransactionDiscount> transactionDiscounts)
        {
            foreach (var item in transactionItems)
            {
                var applicableDiscounts = transactionDiscounts
                    .Where(d => d.Discount.ProductId == item.ProductId && d.Discount.DiscountType == Constants.Discount.Percentage);

                decimal discountedPrice = item.Price; // Start with original unit price

                foreach (var discount in applicableDiscounts)
                {
                    discountedPrice -= (discountedPrice * discount.Discount.DiscountValue / 100); // Apply percentage discount
                }

                // Ensure unit price doesn't go below 0
                discountedPrice = Math.Max(discountedPrice, 0);

                item.Price = discountedPrice; // Update unit price
            }
        }

        private void ApplyMultiBuyDiscount(IEnumerable<TransactionItem> transactionItems, IEnumerable<TransactionDiscount> transactionDiscounts)
        {
            foreach (var discount in transactionDiscounts)
            {
                if (discount.Discount.DiscountType == Constants.Discount.MultiBuy)
                {
                    var applicableItems = transactionItems
                        .Where(item => item.ProductId == discount.Discount.ProductId);

                    foreach (var item in applicableItems)
                    {
                        if (item.Quantity >= discount.Discount.RequiredQuantity)
                        {
                            // Number of times the discount can be applied
                            int applicableTimes = item.Quantity / discount.Discount.RequiredQuantity.Value;

                            // Apply discount per unit price
                            decimal discountPerUnit = discount.Discount.DiscountValue / item.Quantity;
                            item.Price -= discountPerUnit;

                            // Ensure price is non-negative
                            item.Price = Math.Max(item.Price, 0);

                            // Update total price
                            //item.TotalPrice = item.Price * item.Quantity; This is computed!

                            // Handle free item (e.g., buy 3 get 1 free)
                            if (discount.Discount.RequiredProductId.HasValue)
                            {
                                var freeItem = transactionItems.FirstOrDefault(i => i.ProductId == discount.Discount.RequiredProductId.Value);
                                if (freeItem != null)
                                {
                                    freeItem.Quantity += applicableTimes; // Get exactly the applicable free items
                                }
                            }
                        }
                    }
                }
            }
        }

    }

}
