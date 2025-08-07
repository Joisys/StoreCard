using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StoreCard.Data.Interfaces;
using StoreCard.Data.Repository;

namespace StoreCard.Data
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection RegisterDataRepository(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<StoreCardDbContext>((services, o) =>
            {
                o.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                o.EnableSensitiveDataLogging();
            }, optionsLifetime: ServiceLifetime.Singleton);

            services.AddScoped<IStoreCardDbContext, StoreCardDbContext>();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
