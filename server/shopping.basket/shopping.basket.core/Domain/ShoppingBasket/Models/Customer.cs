using System.ComponentModel.DataAnnotations;

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

        public DateTime? LastLogin { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
    }
}
