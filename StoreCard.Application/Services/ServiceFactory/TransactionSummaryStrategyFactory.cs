using Microsoft.Extensions.DependencyInjection;
using StoreCard.Application.Interfaces;
using StoreCard.Domain.Enums;

namespace StoreCard.Application.Services.ServiceFactory
{
    public class TransactionSummaryStrategyFactory : ITransactionSummaryStrategyFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public TransactionSummaryStrategyFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ITransactionSummaryStrategy Create(SummaryType type, decimal? threshold = null)
        {
            return type switch
            {
                SummaryType.TotalPerUser =>
                    _serviceProvider.GetRequiredService<TotalPerUserStrategy>(),

                SummaryType.TotalPerTransactionType =>
                    _serviceProvider.GetRequiredService<TotalPerTransactionTypeStrategy>(),

                SummaryType.HighVolume when threshold.HasValue =>
                    ActivatorUtilities.CreateInstance<HighVolumeTransactionStrategy>(_serviceProvider, threshold.Value),

                SummaryType.HighVolume =>
                    throw new ArgumentException("Threshold is required for HighVolume strategy."),

                _ => throw new NotImplementedException($"Summary type {type} not implemented.")
            };
        }
    }
}
