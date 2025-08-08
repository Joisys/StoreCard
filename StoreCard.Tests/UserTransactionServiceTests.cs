using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using StoreCard.Application.Dtos.UserTransaction;
using StoreCard.Application.Interfaces;
using StoreCard.Application.Services;
using StoreCard.Data.Interfaces;
using StoreCard.Domain.Entities;
using StoreCard.Domain.Enums;

namespace StoreCard.Tests
{
    [TestFixture]
    public class UserTransactionServiceTests
    {
        private IUserTransactionRepository _transactionRepository = null!;
        private IUnitOfWork _unitOfWork = null!;
        private IMapper _mapper = null!;
        private ILogger<UserTransactionService> _logger = null!;
        private ITransactionSummaryStrategyFactory _strategyFactory = null!;
        private ITransactionSummaryStrategy _strategy = null!;
        private UserTransactionService _service = null!;

        [SetUp]
        public void SetUp()
        {
            _transactionRepository = Substitute.For<IUserTransactionRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _mapper = Substitute.For<IMapper>();
            _logger = Substitute.For<ILogger<UserTransactionService>>();
            _strategyFactory = Substitute.For<ITransactionSummaryStrategyFactory>();
            _strategy = Substitute.For<ITransactionSummaryStrategy>();

            _service = new UserTransactionService(
                _transactionRepository,
                _unitOfWork,
                _mapper,
                _logger,
                _strategyFactory
            );
        }

        [Test]
        public async Task GetAllUserTransactionsAsync_ReturnsMappedDtos()
        {
            var transactions = new List<UserTransaction> { new UserTransaction { Id = 100 } };
            var dtos = new List<UserTransactionDto> { new UserTransactionDto { Id = 100 } };

            _transactionRepository.GetUserTransactionsList().Returns(Task.FromResult<IEnumerable<UserTransaction>>(transactions));
            _mapper.Map<IEnumerable<UserTransactionDto>>(transactions).Returns(dtos);

            var result = await _service.GetAllUserTransactionsAsync();

            Assert.That(result, Is.EqualTo(dtos));
        }

        [Test]
        public void GetUserTransactionAsync_WhenNotFound_ThrowsKeyNotFoundException()
        {

            _transactionRepository.GetQueryById(1).Returns(Task.FromResult<UserTransaction?>(null));

            var ex = Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetUserTransactionAsync(1));
            Assert.That(ex!.Message, Does.Contain("not found"));
        }

        [Test]
        public async Task GetUserTransactionAsync_WhenFound_ReturnsDto()
        {
            var transaction = new UserTransaction { Id = 100 };
            var dto = new UserTransactionDto { Id = 100 };

            _transactionRepository.GetQueryById(100).Returns(Task.FromResult<UserTransaction?>(transaction));
            _mapper.Map<UserTransactionDto>(transaction).Returns(dto);


            var result = await _service.GetUserTransactionAsync(100);

            Assert.That(result, Is.EqualTo(dto));
        }

        [Test]
        public void CreateUserTransactionAsync_WhenDtoIsNull_ThrowsArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _service.CreateUserTransactionAsync(null!));
        }

        [Test]
        public async Task CreateUserTransactionAsync_SavesTransactionAndReturnsDto()
        {

            var createDto = new UserTransactionCreateDto { Amount = 50 };
            var transaction = new UserTransaction { Id = 100, Amount = 50 };
            var dto = new UserTransactionDto { Id = 100, Amount = 50 };

            _mapper.Map<UserTransaction>(createDto).Returns(transaction);
            _mapper.Map<UserTransactionDto>(transaction).Returns(dto);

            var result = await _service.CreateUserTransactionAsync(createDto);

            await _transactionRepository.Received(1).AddUserTransactionAsync(transaction);
            await _unitOfWork.Received(1).SaveChangesAsync();
            Assert.That(result, Is.EqualTo(dto));
        }

        [Test]
        public async Task GetTransactionSummaryAsync_UsesStrategyFromFactory()
        {
            var expectedSummaries = new List<UserTransactionSummaryDto>
            {
                new UserTransactionSummaryDto { UserId = 100, TotalAmount = 250 }
            };

            _strategyFactory.Create(SummaryType.TotalPerTransactionType, null).Returns(_strategy);
            _strategy.GetSummaryAsync().Returns(Task.FromResult<IEnumerable<UserTransactionSummaryDto>>(expectedSummaries));
            var result = await _service.GetTransactionSummaryAsync(SummaryType.TotalPerTransactionType);


            Assert.That(result, Is.EqualTo(expectedSummaries));
            _strategyFactory.Received(1).Create(SummaryType.TotalPerTransactionType, null);
            await _strategy.Received(1).GetSummaryAsync();
        }


        [TearDown]
        public void TearDown()
        {
            if (_unitOfWork is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
