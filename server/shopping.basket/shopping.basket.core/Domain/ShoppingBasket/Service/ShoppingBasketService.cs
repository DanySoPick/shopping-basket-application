using shopping.basket.core.Domain.ShoppingBasket.Repository;
using shopping.basket.ShoppingBasket.Models;

namespace shopping.basket.core.Domain.ShoppingBasket.Service
{
    public class ShoppingBasketService : IShoppingBasketService
    {
        private ILogger<ShoppingBasketService> _logger;
        private ICustomerRepository _customerRepository;

        public ShoppingBasketService(ILogger<ShoppingBasketService> logger, ICustomerRepository customerRepository) {
            _logger = logger;
            _customerRepository = customerRepository;
        }

        public async Task<Customer> GetCustomerByEmail(string email)
        {
            _logger.LogInformation("GetCustomer called with email: {email}", email);
            return await _customerRepository.GetCustomerByEmailAsync(email);
        }
    }
}
