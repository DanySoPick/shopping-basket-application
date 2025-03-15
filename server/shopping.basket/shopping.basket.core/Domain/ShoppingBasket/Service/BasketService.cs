using shopping.basket.core.Domain.ShoppingBasket.Models;
using shopping.basket.core.Domain.ShoppingBasket.Repository.Customer;
using shopping.basket.core.Domain.ShoppingBasket.Repository.Products;

namespace shopping.basket.core.Domain.ShoppingBasket.Service
{
    public class BasketService : IBasketService
    {
        private ILogger<BasketService> _logger;
        private ICustomerRepository _customerRepository;
        private IProductRepository _productRepository;

        public BasketService(ILogger<BasketService> logger, ICustomerRepository customerRepository, IProductRepository productRepository) {
            _logger = logger;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
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
    }
}
