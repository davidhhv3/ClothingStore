using ClothingStore.Core.CustomEntities;
using ClothingStore.Core.Interfaces;
using ClothingStore.Core.Services;
using ClothingStore.Infrastructure.Data;
using ClothingStore.Infrastructure.Interfaces;
using ClothingStore.Infrastructure.Options;
using ClothingStore.Infrastructure.Repositories;
using ClothingStore.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClothingStore.Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<PaginationOptions>(options => configuration.GetSection("Pagination").Bind(options));
            services.Configure<PasswordOptions>(options => configuration.GetSection("PasswordOptions").Bind(options));
            return services;
        }

        public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ClothingStoreContext>(options =>
               options.UseSqlServer(configuration.GetConnectionString("ClothingStoreDb"))
           );
            return services;
        }


        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddTransient<IContryService, CountryService>();
            services.AddTransient<ISecurityService, SecurityService>();
            services.AddSingleton<IPasswordService, PasswordService>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            return services;
        }

    }
}
