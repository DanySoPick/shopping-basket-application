using System.ComponentModel.DataAnnotations;

namespace shopping.basket.core.Domain.ShoppingBasket.Models
{
    public class Product : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public decimal Tax { get; set; }
    }
}
