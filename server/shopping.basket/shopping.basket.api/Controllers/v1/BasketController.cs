using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using shopping.basket.core.Domain.ShoppingBasket;

namespace shopping.basket.api.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class BasketController : ControllerBase
    {
        private readonly IShoppingBasketService _shoppingBasketService;

        public BasketController(IShoppingBasketService shoppingBasketService)
        {
            _shoppingBasketService = shoppingBasketService;
        }

        /// <summary>
        /// Return customer by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet("{email}")]
        public async Task<ActionResult<string>> GetCustomerByEmail(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    return BadRequest("Email is required");
                }

                return Ok(await _shoppingBasketService.GetCustomerByEmail(email));
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while geting the subscription: {ex.Message}");
            }
        }
    }
}
