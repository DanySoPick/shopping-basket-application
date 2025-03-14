using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using shopping.basket.core.Domain.ShoppingBasket;
using shopping.basket.core.Domain.ShoppingBasket.Repository;
using shopping.basket.core.Domain.ShoppingBasket.Service;
using shopping.basket.data.Models;
using shopping.basket.data.Repositories;
using shopping.basket.ShoppingBasket.Models;

namespace shopping.basket.core
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddCoreFeatures(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddHttpContextAccessor();
            services.AddDbContext<GenericRepository>(options =>
                options.UseMySql(configuration.GetConnectionString("DefaultConnection"),
                new MySqlServerVersion(new Version(8, 0, 31))));

            services.TryAddScoped<ICustomerRepository, CustomerRepository>();

            services.TryAddScoped<IShoppingBasketService, ShoppingBasketService>();
            services.AddHttpContextAccessor();

            return services;
        }
    }
}
