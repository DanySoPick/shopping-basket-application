using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace shopping.basket.core.Domain.ShoppingBasket.Models
{
    public class Discount : BaseEntity
    {
        [Required]
        [MaxLength(255)]
        [Column("discount_name")]
        public string DiscountName { get; set; }

        [Column("product_id")]
        public int? ProductId { get; set; } // Nullable for global discounts

        [Required]
        [Column("discount_type")]
        public string DiscountType { get; set; } // ENUM('PERCENTAGE', 'MULTI_BUY')

        [Required]
        [Column("discount_value")]
        public decimal DiscountValue { get; set; } // Percentage or price reduction

        [Column("required_product_id")]
        public int? RequiredProductId { get; set; } // Needed for multi-buy

        [Column("required_quantity")]
        public int? RequiredQuantity { get; set; } // Minimum quantity for multi-buy

        [Column("start_date")]
        public DateTime StartDate { get; set; }

        [Column("end_date")]
        public DateTime EndDate { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [ForeignKey("RequiredProductId")]
        public Product RequiredProduct { get; set; }
    }
}
