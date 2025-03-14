using System.ComponentModel.DataAnnotations;

namespace shopping.basket.ShoppingBasket.Models
{
    public class Product
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
