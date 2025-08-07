using StoreCard.Application.Dtos.UserTransaction;
using StoreCard.Application.Interfaces;
using StoreCard.Data.Interfaces;

namespace StoreCard.Application.Services.ServiceFactory
{
    public class TotalPerUserStrategy : ITransactionSummaryStrategy
    {
        private readonly IUserTransactionRepository _repository;

        public TotalPerUserStrategy(IUserTransactionRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<UserTransactionSummaryDto>> GetSummaryAsync()
        {
            var transactions = await _repository.GetUserTransactionsList();
            return transactions
                .GroupBy(t => t.UserId)
                .Select(g => new UserTransactionSummaryDto
                {
                    UserId = g.Key,
                    TransactionType = "All",
                    TotalAmount = g.Sum(x => x.Amount)
                })
                .ToList();
        }
    }
}
