using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace shopping.basket.core.Domain.ShoppingBasket.Models
{
    public class TransactionDiscount
    {
        [Required]
        public int TransactionId { get; set; }

        [Required]
        public int DiscountId { get; set; }

        [Required]
        public decimal DiscountApplied { get; set; }

        [ForeignKey("TransactionId")]
        public Transaction Transaction { get; set; }

        [ForeignKey("DiscountId")]
        public Discount Discount { get; set; }
    }
}
