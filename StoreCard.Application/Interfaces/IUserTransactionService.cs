using StoreCard.Application.Dtos.UserTransaction;

namespace StoreCard.Application.Interfaces
{
    public interface IUserTransactionService
    {
        Task<UserTransactionDto> GetUserTransactionAsync(int id);
        Task<IEnumerable<UserTransactionDto>> GetAllUserTransactionsAsync();
        Task<UserTransactionDto> CreateUserTransactionAsync(UserTransactionCreateDto dto);

        Task<IEnumerable<UserTransactionSummaryDto>> GetUserTransactionSummaryAsync(int? userId = null, string? transactionType = null, decimal? threshold = 0);
    }
}
