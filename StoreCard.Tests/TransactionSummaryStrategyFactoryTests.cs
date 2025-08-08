using NSubstitute;
using StoreCard.Application.Services.ServiceFactory;
using StoreCard.Data.Interfaces;
using StoreCard.Domain.Enums;

namespace StoreCard.Tests
{
    [TestFixture]
    public class TransactionSummaryStrategyFactoryTests
    {
        private IServiceProvider _serviceProvider = null!;
        private TransactionSummaryStrategyFactory _factory = null!;

        private TotalPerUserStrategy _totalPerUserStrategy = null!;
        private TotalPerTransactionTypeStrategy _totalPerTransactionTypeStrategy = null!;

        [SetUp]
        public void SetUp()
        {
            _serviceProvider = Substitute.For<IServiceProvider>();

            var userTransactionRepository = Substitute.For<IUserTransactionRepository>();

            _totalPerUserStrategy = new TotalPerUserStrategy(userTransactionRepository);
            _totalPerTransactionTypeStrategy = new TotalPerTransactionTypeStrategy(userTransactionRepository);

            _factory = new TransactionSummaryStrategyFactory(_serviceProvider);

            _serviceProvider.GetService(typeof(TotalPerUserStrategy))
                .Returns(_totalPerUserStrategy);

            _serviceProvider.GetService(typeof(TotalPerTransactionTypeStrategy))
                .Returns(_totalPerTransactionTypeStrategy);
        }

        [Test]
        public void Create_TotalPerUser_ReturnsCorrectStrategy()
        {
            var result = _factory.Create(SummaryType.TotalPerUser);
            Assert.That(result, Is.SameAs(_totalPerUserStrategy));
        }

        [Test]
        public void Create_TotalPerTransactionType_ReturnsCorrectStrategy()
        {
            var result = _factory.Create(SummaryType.TotalPerTransactionType);
            Assert.That(result, Is.SameAs(_totalPerTransactionTypeStrategy));
        }

        [Test]
        public void Create_HighVolumeTransaction_WithThreshold_ReturnsCorrectStrategy()
        {
            var userTransactionRepository = Substitute.For<IUserTransactionRepository>();
            _serviceProvider.GetService(typeof(IUserTransactionRepository))
                            .Returns(userTransactionRepository);

            var factory = new TransactionSummaryStrategyFactory(_serviceProvider);

            var result = factory.Create(SummaryType.HighVolume, 100m);

            Assert.That(result, Is.TypeOf<HighVolumeTransactionStrategy>());
        }

    }

}
