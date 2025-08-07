using Microsoft.Extensions.DependencyInjection;
using StoreCard.Application.Interfaces;
using StoreCard.Application.Profiles;
using StoreCard.Application.Services;
using StoreCard.Application.Services.ServiceFactory;

namespace StoreCard.Application
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();

            services.AddTransient<ITransactionSummaryStrategyFactory, TransactionSummaryStrategyFactory>();
            services.AddScoped<TotalPerUserStrategy>();
            services.AddScoped<TotalPerTransactionTypeStrategy>();

            services.AddScoped<IUserTransactionService, UserTransactionService>();

            services.AddAutoMapper(cfg => { }, typeof(UserMappingProfile));
            services.AddAutoMapper(cfg => { }, typeof(UserTransactionMappingProfile));
            return services;
        }
    }
}
