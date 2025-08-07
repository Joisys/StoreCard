using StoreCard.Domain.Enums;

namespace StoreCard.Application.Interfaces
{
    public interface ITransactionSummaryStrategyFactory
    {
        ITransactionSummaryStrategy Create(SummaryType type, decimal? threshold = null);
    }
}
