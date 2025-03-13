using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace shopping.basket.ShoppingBasket.Models
{
    public class TransactionItem
    {
        [Required]
        public int TransactionId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        public decimal TotalPrice => Quantity * Price; // Computed property

        [ForeignKey("TransactionId")]
        public Transaction Transaction { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}
