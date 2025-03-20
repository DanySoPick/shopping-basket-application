using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace shopping.basket.core.Domain.ShoppingBasket.Models
{
    [Table("Transaction_Items")]
    public class TransactionItem : BaseEntity
    {
        [Required]
        [Column("transaction_id")]
        public int TransactionId { get; set; }

        [Required]
        [Column("product_id")]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Column("total_price")]
        public decimal TotalPrice => Quantity * Price; // Computed property

        [ForeignKey("TransactionId")]
        public Transaction Transaction { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}
