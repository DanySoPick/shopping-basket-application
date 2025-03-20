using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using shopping.basket.api.DTO;
using System.Text;
using System.Text.Json;

namespace shopping.basket.xtest
{
        public class BasketControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public BasketControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/transaction")]
        [InlineData("/products")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task GetCustomerByEmailAsync_ReturnsOkResult_WhenCustomerExists()
        {
            // Arrange
            var email = "test@example.com";
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync($"/api/Basket/{email}");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains(email, responseString);
        }

        [Fact]
        public async Task NewTransactionAsync_ReturnsOkResult_WhenTransactionIsValid()
        {
            // Arrange
            var client = _factory.CreateClient();
            var basketDTO = new BasketDTO
            {
                CustomerId = 1,
                Items = new List<SelectedItemsDTO>
                {
                    new SelectedItemsDTO { ProductId = 1, Quantity = 2 }
                },
                Discounts = new List<SelectedDiscountDTO>
                {
                    new SelectedDiscountDTO { DiscountId = 1 }
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(basketDTO), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PutAsync("/transaction", content);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("CustomerId", responseString);
        }
    }
}
