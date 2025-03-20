using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using shopping.basket.api.DTO;
using shopping.basket.core.Domain.ShoppingBasket.Models;
using shopping.basket.core.Domain.ShoppingBasket.Service;

namespace shopping.basket.api.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _shoppingBasketService;
        private readonly IMapper _mapper;

        public BasketController(IBasketService shoppingBasketService, IMapper mapper)
        {
            _shoppingBasketService = shoppingBasketService;
            _mapper = mapper;
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
        /// Return all store products and discounts if existing
        /// </summary>
        /// <returns>products</returns>
        [HttpGet("/products")]
        public async Task<ActionResult<ProductBundleDto>> GetProductsAsync()
        {
            try
            {
                var products = await _shoppingBasketService.GetProductsAsync();
                if (products == null)
                    return NotFound($"Products not found.");

                //we can have empty discounts
                var discounts = await _shoppingBasketService.GetAvailableDiscountsAsync(DateTime.Now);

                var bundle = new ProductBundleDto
                {
                    Products = _mapper.Map<IEnumerable<ProductDTO>>(products),
                    Discounts = discounts == null ? new List<DiscountDTO>() : _mapper.Map<IEnumerable<DiscountDTO>>(discounts)
                };

                return Ok(bundle);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        /// <summary>
        /// Get available discounts
        /// </summary>
        /// <returns></returns>
        [HttpGet("/discounts")]
        public async Task<ActionResult<IEnumerable<DiscountDTO>>> GetAvailableDiscountsAsync()
        {
            try
            { 
                // TODO: create store procedure to return products and discounts in 1 call
                var discounts = await _shoppingBasketService.GetAvailableDiscountsAsync(DateTime.Now);

                var mapDiscounts = _mapper.Map<IEnumerable<DiscountDTO>>(discounts);
                
                if (mapDiscounts == null)
                    return NotFound($"Customer with email {mapDiscounts} not found.");

                return Ok(mapDiscounts);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        /// <summary>
        /// Insert a transaction
        /// </summary>
        /// <returns>products</returns>
        [HttpPost("/transaction/save")]
        public async Task<ActionResult<Transaction>> NewTransactionAsync(BasketDTO basketPurchase)
        {
            try
            {
                if (basketPurchase == null)
                {
                    return BadRequest("Basket purchase is required");
                }

                var Items = _mapper.Map<IEnumerable<TransactionItem>>(basketPurchase.Items);
                var Discounts = _mapper.Map<IEnumerable<TransactionDiscount>>(basketPurchase.Discounts);

                var transaction = await _shoppingBasketService.InsertTransaction(basketPurchase.CustomerId, Items, Discounts);

                if (transaction == null)
                    return NotFound($"Products not found.");

                var transactionSimplefied = _mapper.Map<TransactionDTO>(transaction);

                return Ok(transactionSimplefied);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        /// <summary>
        /// Return final basket with discounts
        /// </summary>
        /// <returns>products</returns>
        [HttpPost("/transaction/checkOut")]
        public async Task<ActionResult<TransactionSummaryDTO>> CalculateCheckOutAsync(BasketDTO basketPurchase)
        {
            try // TODO: not finished
            {
                if (basketPurchase == null)
                {
                    return BadRequest("Basket purchase is required");
                }

                var Items = _mapper.Map<IEnumerable<TransactionItem>>(basketPurchase.Items);
                var Discounts = _mapper.Map<IEnumerable<TransactionDiscount>>(basketPurchase.Discounts);

                var transaction = await _shoppingBasketService.CalculateCheckOutAsync(Items, Discounts);

                if (transaction == null)
                    return NotFound($"Products not found.");

                var transactionSimplefied = _mapper.Map<TransactionSummaryDTO>(transaction);

                return Ok(transactionSimplefied);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }
    }
}
