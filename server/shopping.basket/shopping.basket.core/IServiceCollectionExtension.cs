using Microsoft.EntityFrameworkCore;
using shopping.basket.data.Repositories;

namespace shopping.basket.core
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddCoreFeatures(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddDbContext<ShoppingBasketContext>(options =>
                options.UseMySql(configuration.GetConnectionString("ShoppingBasketDatabase"),
                new MySqlServerVersion(new Version(8, 0, 31))));

            services.AddHttpContextAccessor();

            return services;
        }
    }
}
