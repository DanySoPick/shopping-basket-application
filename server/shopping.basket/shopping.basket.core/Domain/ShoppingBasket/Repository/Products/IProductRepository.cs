using shopping.basket.core.Domain.ShoppingBasket.Models;
using shopping.basket.data.Repositories;

namespace shopping.basket.core.Domain.ShoppingBasket.Repository.Products
{
    public interface IProductRepository : IGenericRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync();

        Task<Product> FindProductAsync(int id);

        Task<Product> InsertNewProductAsync(Product product);

        Task<Product> UpdateProductAsync(Product product);

        Task<IEnumerable<Discount>> GetAvailableDiscountsAsync(DateTime date);
    }
}
