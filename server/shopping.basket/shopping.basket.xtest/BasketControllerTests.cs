using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using shopping.basket.api.Controllers.v1;
using shopping.basket.api.DTO;
using shopping.basket.core.Domain.ShoppingBasket.Models;
using shopping.basket.core.Domain.ShoppingBasket.Service;

namespace shopping.basket.xtest
{
    public class BasketControllerTests
    {
        private readonly Mock<IBasketService> _mockBasketService;
        private readonly IMapper _mapper;
        private readonly BasketController _controller;

        public BasketControllerTests()
        {
            _mockBasketService = new Mock<IBasketService>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BasketDTO, Transaction>()
                    .ForMember(dest => dest.TransactionItems, opt => opt.MapFrom(src => src.Items))
                    .ForMember(dest => dest.TransactionDiscounts, opt => opt.MapFrom(src => src.Discounts));
                cfg.CreateMap<SelectedItemsDTO, TransactionItem>();
                cfg.CreateMap<currentDiscountDTO, TransactionDiscount>();
                cfg.CreateMap<Transaction, TransactionDTO>();
            });

            _mapper = config.CreateMapper();
            _controller = new BasketController(_mockBasketService.Object, _mapper);
        }

        [Fact]
        public async Task GetCustomerByEmailAsync_ReturnsOkResult_WhenCustomerExists()
        {
            // Arrange
            var email = "test@example.com";
            var customer = new Customer { Id = 1, Name = "Test Customer", Email = email };
            _mockBasketService.Setup(service => service.GetCustomerByEmailAsync(email))
                .ReturnsAsync(customer);

            // Act
            var result = await _controller.GetCustomerByEmailAsync(email);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedCustomer = Assert.IsType<Customer>(okResult.Value);
            Assert.Equal(email, returnedCustomer.Email);
        }

        [Fact]
        public async Task NewTransactionAsync_ReturnsOkResult_WhenTransactionIsValid()
        {
            // Arrange
            var basketDTO = new BasketDTO
            {
                CustomerId = 1,
                Items = new List<SelectedItemsDTO>
                {
                    new SelectedItemsDTO { ProductId = 1, Quantity = 2 }
                },
                Discounts = new List<currentDiscountDTO>
                {
                    new currentDiscountDTO { DiscountId = 1 }
                }
            };

            var transaction = new Transaction
            {
                CustomerId = 1,
                TransactionItems = new List<TransactionItem>
                {
                    new TransactionItem { ProductId = 1, Quantity = 2, Price = 100 }
                },
                TransactionDiscounts = new List<TransactionDiscount>
                {
                    new TransactionDiscount { DiscountId = 1 }
                }
            };

            _mockBasketService.Setup(service => service.InsertTransaction(It.IsAny<int>(), It.IsAny<IEnumerable<TransactionItem>>(), It.IsAny<IEnumerable<TransactionDiscount>>()))
                .ReturnsAsync(transaction);

            // Act
            var result = await _controller.NewTransactionAsync(basketDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedTransaction = Assert.IsType<TransactionDTO>(okResult.Value);
            Assert.Equal(transaction.Id, returnedTransaction.Id);
        }
    }
}
