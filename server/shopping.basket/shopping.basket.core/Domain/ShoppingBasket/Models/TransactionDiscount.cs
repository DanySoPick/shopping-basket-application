using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace shopping.basket.core.Domain.ShoppingBasket.Models
{
    [Table("Transaction_Discounts")]
    public class TransactionDiscount : BaseEntity
    {
        [Required]
        [Column("transaction_id")]
        public int TransactionId { get; set; }

        [Required]
        [Column("discount_id")]
        public int DiscountId { get; set; }

        [Required]
        [Column("discount_applied")]
        public decimal DiscountApplied { get; set; }

        [ForeignKey("TransactionId")]
        public Transaction Transaction { get; set; }

        [ForeignKey("DiscountId")]
        public Discount Discount { get; set; }
    }
}
