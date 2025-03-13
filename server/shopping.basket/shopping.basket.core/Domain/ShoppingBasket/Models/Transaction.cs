using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace shopping.basket.ShoppingBasket.Models
{
    public class Transaction
    {

        [Required]
        public int CustomerId { get; set; }

        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        public ICollection<TransactionItem> TransactionItems { get; set; }

        public ICollection<TransactionDiscount> TransactionDiscounts { get; set; }
    }
}
