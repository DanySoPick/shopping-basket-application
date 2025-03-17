using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using shopping.basket.core.Domain.ShoppingBasket.Repository.Customer;
using shopping.basket.core.Domain.ShoppingBasket.Repository.Customers;
using shopping.basket.core.Domain.ShoppingBasket.Repository.Products;
using shopping.basket.core.Domain.ShoppingBasket.Repository.Transactions;
using shopping.basket.core.Domain.ShoppingBasket.Service;
using shopping.basket.data.Repositories;

namespace shopping.basket.core
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddCoreFeatures(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
            });
            services.AddHttpContextAccessor();
            services.AddDbContext<GenericRepository>(options =>
                options.UseMySql(configuration.GetConnectionString("DefaultConnection"),
                new MySqlServerVersion(new Version(8, 0, 31))));

            services.TryAddScoped<ICustomerRepository, CustomerRepository>();
            services.TryAddScoped<IProductRepository, ProductRepository>();
            services.TryAddScoped<ITransactionRepository, TransactionRepository>();

            services.TryAddScoped<IBasketService, BasketService>();
            services.AddHttpContextAccessor();

            return services;
        }
    }
}
