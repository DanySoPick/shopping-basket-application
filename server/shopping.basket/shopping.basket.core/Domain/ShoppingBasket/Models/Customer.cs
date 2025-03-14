using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace shopping.basket.ShoppingBasket.Models
{
    public class Customer : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Column("last_login")]
        public DateTime? LastLogin { get; set; }

       // public ICollection<Transaction> Transactions { get; set; }
    }
}
