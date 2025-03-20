using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace shopping.basket.core.Domain.ShoppingBasket.Models
{
    public class Transaction : BaseEntity
    {

        [Required]
        [Column("customer_id")]
        public int CustomerId { get; set; }

        [Column("transaction_date")]
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        public ICollection<TransactionItem> TransactionItems { get; set; }

        public ICollection<TransactionDiscount> TransactionDiscounts { get; set; }
    }
}
