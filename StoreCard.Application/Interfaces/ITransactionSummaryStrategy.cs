using StoreCard.Application.Dtos.UserTransaction;

namespace StoreCard.Application.Interfaces
{
    public interface ITransactionSummaryStrategy
    {
        Task<IEnumerable<UserTransactionSummaryDto>> GetSummaryAsync();
    }
}
