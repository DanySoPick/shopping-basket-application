using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using shopping.basket.core.Domain.ShoppingBasket.Service;

namespace shopping.basket.api.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _shoppingBasketService;

        public BasketController(IBasketService shoppingBasketService)
        {
            _shoppingBasketService = shoppingBasketService;
        }

        /// <summary>
        /// Return customer by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet("{email}")]
        public async Task<ActionResult<string>> GetCustomerByEmailAsync(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    return BadRequest("Email is required");
                }
                var customer = await _shoppingBasketService.GetCustomerByEmailAsync(email);

                if (customer == null)
                    return NotFound($"Customer with email {email} not found.");

                return Ok(customer);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        /// <summary>
        /// Return all store products
        /// </summary>
        /// <returns>products</returns>
        [HttpGet("/products")]
        public async Task<ActionResult<string>> GetProductsAsync()
        {
            try
            {
                var products = await _shoppingBasketService.GetProductsAsync();

                if(products == null)
                    return NotFound($"Products not found.");

                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }
    }
}
