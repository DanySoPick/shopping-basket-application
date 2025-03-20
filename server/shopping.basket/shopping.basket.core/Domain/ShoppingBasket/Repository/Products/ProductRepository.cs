using Microsoft.EntityFrameworkCore;
using shopping.basket.core.Domain.ShoppingBasket.Models;
using shopping.basket.data.Repositories;

namespace shopping.basket.core.Domain.ShoppingBasket.Repository.Products
{
    public class ProductRepository : GenericRepository, IProductRepository
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Discount> Discounts { get; set; }

        public ProductRepository(DbContextOptions<GenericRepository> options, IConfiguration configuration)
            : base(options, configuration)
        {
        }

        public async Task<IEnumerable<Product>> GetProductsAsync() 
        {
            return await Products.ToListAsync();
        }

        public async Task<Product> FindProductAsync(int id)
        {
            var product = await Products.SingleOrDefaultAsync(i => i.Id == id);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {id} not found.");
            }
            return product;
        }

        public async Task<Product> InsertNewProductAsync(Product product)
        {
            throw new NotImplementedException("InsertNewProductAsync method is not implemented yet.");
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            throw new NotImplementedException("UpdateProductAsync method is not implemented yet.");
        }

        public async Task<IEnumerable<Discount>> GetAvailableDiscountsAsync(DateTime date)
        {
            return await Discounts.Where(d => d.StartDate <= date && d.EndDate >= date).ToListAsync();
        }

    }
}
