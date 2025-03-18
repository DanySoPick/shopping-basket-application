using shopping.basket.core.Domain.ShoppingBasket.Models;

namespace shopping.basket.api.DTO
{
    public class BasketDTO
    {
        public int CustomerId { get; set; }
        public IEnumerable<SelectedItemsDTO> Items { get; set; }
        public IEnumerable<SelectedDiscountDTO> Discounts { get; set; }
    }

    public class SelectedItemsDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class SelectedDiscountDTO
    {
        public int DiscountId { get; set; }
    }

    public class DiscountDTO
    {
        public int Id { get; set; } //discount id
        public int? ProductId { get; set; }
        public string DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public int? RequiredProductId { get; set; }
        public int? RequiredQuantity { get; set; }

    }

    public class TransactionDTO
    {
        public int Id { get; set; } // The TransactionId
        public int CustomerId { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
