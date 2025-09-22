using StoreCard.Application.Dtos.UserTransaction;
using StoreCard.Domain.Enums;

namespace StoreCard.Application.Interfaces
{
    public interface IUserTransactionService
    {
        Task<UserTransactionDto> GetUserTransactionAsync(int id);
        Task<IEnumerable<UserTransactionDto>> GetAllUserTransactionsAsync();
        Task<UserTransactionDto> CreateUserTransactionAsync(UserTransactionDto dto);

        Task<IEnumerable<UserTransactionSummaryDto>> GetTransactionSummaryAsync(SummaryType type, decimal? threshold = null);

    }
}
