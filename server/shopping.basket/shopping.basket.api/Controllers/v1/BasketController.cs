using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace shopping.basket.api.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class BasketController : ControllerBase
    {
        /// <summary>
        /// Return audiences by tenant Id
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        [HttpGet("{tenantId}")]
        public async Task<ActionResult<string>> Get(string tenantId)
        {
            try
            {
                
                return Ok(tenantId);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while geting the subscription: {ex.Message}");
            }
        }
    }
}
