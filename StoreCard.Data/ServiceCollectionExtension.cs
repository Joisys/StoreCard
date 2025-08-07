using Microsoft.Extensions.DependencyInjection;
using StoreCard.Data.Interfaces;
using StoreCard.Data.Repository;

namespace StoreCard.Data
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection RegisterDataRepository(this IServiceCollection services)
        {

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
