using shopping.basket.core.Domain.ShoppingBasket.Models;

namespace shopping.basket.api.DTO
{
    public class BasketDTO
    {
        public int CustomerId { get; set; }
        public List<SelectedItemsDTO> Items { get; set; }
        public List<SelectedDiscountDTO> Discounts { get; set; }
    }

    public class ProductBundleDto
    {
        public IEnumerable<ProductDTO> Products { get; set; }
        public IEnumerable<DiscountDTO> Discounts { get; set; }
    }

    

    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
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

    public class TransactionSummaryDTO
    {
        public decimal TotalPrice { get; set; }
    }
}
