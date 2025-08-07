using Microsoft.Extensions.DependencyInjection;
using StoreCard.Application.Interfaces;
using StoreCard.Application.Profiles;
using StoreCard.Application.Services;

namespace StoreCard.Application
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserTransactionService, UserTransactionService>();

            services.AddAutoMapper(cfg => { }, typeof(UserMappingProfile));
            services.AddAutoMapper(cfg => { }, typeof(UserTransactionMappingProfile));
            return services;
        }
    }
}
