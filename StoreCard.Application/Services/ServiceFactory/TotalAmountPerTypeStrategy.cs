using StoreCard.Application.Dtos.UserTransaction;
using StoreCard.Application.Interfaces;
using StoreCard.Data.Interfaces;

namespace StoreCard.Application.Services.ServiceFactory
{
    public class TotalPerTransactionTypeStrategy : ITransactionSummaryStrategy
    {
        private readonly IUserTransactionRepository _repository;

        public TotalPerTransactionTypeStrategy(IUserTransactionRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<UserTransactionSummaryDto>> GetSummaryAsync()
        {
            var transactions = await _repository.GetUserTransactionsList();
            return transactions
                .GroupBy(t => t.Type)
                .Select(g => new UserTransactionSummaryDto
                {
                    UserId = 0,
                    TransactionType = g.Key.ToString(),
                    TotalAmount = g.Sum(x => x.Amount)
                })
                .ToList();
        }
    }

}
