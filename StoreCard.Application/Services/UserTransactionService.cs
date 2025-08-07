using AutoMapper;
using Microsoft.Extensions.Logging;
using StoreCard.Application.Dtos.UserTransaction;
using StoreCard.Application.Interfaces;
using StoreCard.Data.Interfaces;
using StoreCard.Domain.Entities;
using StoreCard.Domain.Enums;

namespace StoreCard.Application.Services
{
    public class UserTransactionService : IUserTransactionService
    {
        private readonly IUserTransactionRepository _userTransactionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UserTransactionService> _logger;

        private readonly ITransactionSummaryStrategyFactory _strategyFactory;

        public UserTransactionService(
            IUserTransactionRepository transactionRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<UserTransactionService> logger,
            ITransactionSummaryStrategyFactory strategyFactory)
        {
            _userTransactionRepository = transactionRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _strategyFactory = strategyFactory;
        }

        public async Task<IEnumerable<UserTransactionDto>> GetAllUserTransactionsAsync()
        {
            var transactions = await _userTransactionRepository.GetUserTransactionsList();
            return _mapper.Map<IEnumerable<UserTransactionDto>>(transactions);
        }

        public async Task<UserTransactionDto> GetUserTransactionAsync(int id)
        {
            var transaction = await _userTransactionRepository.GetQueryById(id);
            if (transaction == null)
            {
                _logger.LogWarning("Transaction with Id {TransactionId} not found.", id);
                throw new KeyNotFoundException($"Transaction with Id {id} not found.");
            }

            return _mapper.Map<UserTransactionDto>(transaction);
        }

        public async Task<UserTransactionDto> CreateUserTransactionAsync(UserTransactionCreateDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var transaction = _mapper.Map<UserTransaction>(dto);
            await _userTransactionRepository.AddUserTransactionAsync(transaction);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Created new transaction with Id {TransactionId}", transaction.Id);

            return _mapper.Map<UserTransactionDto>(transaction);
        }

        public async Task<IEnumerable<UserTransactionSummaryDto>> GetTransactionSummaryAsync(SummaryType type, decimal? threshold = null)
        {
            var strategy = _strategyFactory.Create(type, threshold);
            return await strategy.GetSummaryAsync();
        }

    }

}
