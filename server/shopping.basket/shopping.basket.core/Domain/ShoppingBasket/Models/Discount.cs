using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace shopping.basket.ShoppingBasket.Models
{
    public class Discount : BaseEntity
    {
        [Required]
        [MaxLength(255)]
        public string DiscountName { get; set; }

        public int? ProductId { get; set; } // Nullable for global discounts

        [Required]
        public string DiscountType { get; set; } // ENUM('PERCENTAGE', 'MULTI_BUY')

        [Required]
        public decimal DiscountValue { get; set; } // Percentage or price reduction

        public int? RequiredProductId { get; set; } // Needed for multi-buy

        public int? RequiredQuantity { get; set; } // Minimum quantity for multi-buy

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [ForeignKey("RequiredProductId")]
        public Product RequiredProduct { get; set; }
    }
}
