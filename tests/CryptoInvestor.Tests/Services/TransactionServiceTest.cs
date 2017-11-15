using AutoMapper;
using CryptoInvestor.Core.Domain;
using CryptoInvestor.Core.Repositories;
using CryptoInvestor.Infrastructure.DTO;
using CryptoInvestor.Infrastructure.Exceptions;
using CryptoInvestor.Infrastructure.Services;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CryptoInvestor.Tests.Services
{
    public class TransactionServiceTest
    {
        private readonly Mock<ITransactionRepository> _transactionRepository;
        private readonly Mock<IPortfolioRepository> _portfolioReposiotory;
        private readonly Mock<ICoinRepository> _coinRepository;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IMapper> _mapper;

        private Coin coin;
        private Transaction transaction;

        public TransactionServiceTest()
        {
            _transactionRepository = new Mock<ITransactionRepository>();
            _portfolioReposiotory = new Mock<IPortfolioRepository>();
            _coinRepository = new Mock<ICoinRepository>();
            _userRepository = new Mock<IUserRepository>();
            _mapper = new Mock<IMapper>();

            coin = new Coin("btc", "bitcoin");
            coin.AddPrice(CoinPrice.Create("USD", 1000M));
            transaction = new Transaction(Guid.NewGuid(), Guid.NewGuid(), "default", coin, 10M,
                DateTimeOffset.UtcNow.ToUnixTimeSeconds(), 10, "USD");
        }

        [Fact]
        public async Task GetAsync_should_invoke_GetAsync_on_repository()
        {
            _transactionRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(transaction);
            _coinRepository.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(coin);
            var transactionService = new TransactionService(_transactionRepository.Object, _portfolioReposiotory.Object,
                _userRepository.Object, _coinRepository.Object, _mapper.Object);

            await transactionService.GetAsync(Guid.NewGuid());

            _transactionRepository.Verify(x => x.GetAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task GetAsync_should_calculate_profit_before_return()
        {
            _transactionRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(transaction);
            _coinRepository.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(coin);
            var transactionService = new TransactionService(_transactionRepository.Object, _portfolioReposiotory.Object,
                _userRepository.Object, _coinRepository.Object, _mapper.Object);

            await transactionService.GetAsync(Guid.NewGuid());

            _coinRepository.Verify(x => x.GetAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task GetAsync_should_return_TransactionDto()
        {
            _transactionRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(transaction);
            _coinRepository.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(coin);
            _mapper.Setup(x => x.Map<TransactionDto>(It.IsAny<Transaction>())).Returns(new TransactionDto());
            var transactionService = new TransactionService(_transactionRepository.Object, _portfolioReposiotory.Object,
                _userRepository.Object, _coinRepository.Object, _mapper.Object);

            var transactionDto = await transactionService.GetAsync(Guid.NewGuid());

            transactionDto.ShouldBeOfType<TransactionDto>();
        }

        [Fact]
        public async Task BrowseAsync_should_invoke_BrowseAsync_on_repository()
        {
            _transactionRepository.Setup(x => x.BrowseAsync(It.IsAny<Guid>())).ReturnsAsync(new[] { transaction });
            _coinRepository.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(coin);
            var transactionService = new TransactionService(_transactionRepository.Object, _portfolioReposiotory.Object,
                _userRepository.Object, _coinRepository.Object, _mapper.Object);

            await transactionService.BrowseAsync(Guid.NewGuid());

            _transactionRepository.Verify(x => x.BrowseAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task BrowseAsync_should_calculate_profit_before_return()
        {
            _transactionRepository.Setup(x => x.BrowseAsync(It.IsAny<Guid>())).ReturnsAsync(new[] { transaction });
            _coinRepository.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(coin);
            var transactionService = new TransactionService(_transactionRepository.Object, _portfolioReposiotory.Object,
                _userRepository.Object, _coinRepository.Object, _mapper.Object);

            await transactionService.BrowseAsync(Guid.NewGuid());

            _coinRepository.Verify(x => x.GetAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task BrowseAsync_should_return_IEnumerable_collection_of_TransactionDto()
        {
            _transactionRepository.Setup(x => x.BrowseAsync(It.IsAny<Guid>())).ReturnsAsync(new[] { transaction });
            _coinRepository.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(coin);
            _mapper.Setup(x => x.Map<IEnumerable<TransactionDto>>(It.IsAny<IEnumerable<Transaction>>()))
                .Returns(new[] { new TransactionDto() });
            var transactionService = new TransactionService(_transactionRepository.Object, _portfolioReposiotory.Object,
                _userRepository.Object, _coinRepository.Object, _mapper.Object);

            var transactionDtos = await transactionService.BrowseAsync(Guid.NewGuid());

            transactionDtos.ShouldBeAssignableTo<IEnumerable<TransactionDto>>();
        }

        [Fact]
        public async Task BrowseAsync_by_portfolioNameId_should_invoke_BrowseAsync_on_repository()
        {
            _transactionRepository.Setup(x => x.BrowseAsync(It.IsAny<Guid>())).ReturnsAsync(new[] { transaction });
            _coinRepository.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(coin);
            var transactionService = new TransactionService(_transactionRepository.Object, _portfolioReposiotory.Object,
                _userRepository.Object, _coinRepository.Object, _mapper.Object);

            await transactionService.BrowseAsync(Guid.NewGuid(), "default");

            _transactionRepository.Verify(x => x.BrowseAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task BrowseAsync_by_portfolioNameId_should_calculate_profit_before_return()
        {
            _transactionRepository.Setup(x => x.BrowseAsync(It.IsAny<Guid>())).ReturnsAsync(new[] { transaction });
            _coinRepository.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(coin);
            var transactionService = new TransactionService(_transactionRepository.Object, _portfolioReposiotory.Object,
                _userRepository.Object, _coinRepository.Object, _mapper.Object);

            await transactionService.BrowseAsync(Guid.NewGuid(), "default");

            _coinRepository.Verify(x => x.GetAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task BrowseAsync_by_portfolioNameId_should_return_IEnumerable_collection_of_TransactionDto()
        {
            _transactionRepository.Setup(x => x.BrowseAsync(It.IsAny<Guid>())).ReturnsAsync(new[] { transaction });
            _coinRepository.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(coin);
            _mapper.Setup(x => x.Map<IEnumerable<TransactionDto>>(It.IsAny<IEnumerable<Transaction>>()))
                .Returns(new[] { new TransactionDto() });
            var transactionService = new TransactionService(_transactionRepository.Object, _portfolioReposiotory.Object,
                _userRepository.Object, _coinRepository.Object, _mapper.Object);

            var transactionDtos = await transactionService.BrowseAsync(Guid.NewGuid(), "default");

            transactionDtos.ShouldBeAssignableTo<IEnumerable<TransactionDto>>();
        }

        [Fact]
        public async Task CreateAsync_should_invoke_AddAsync_on_repository()
        {
            var user = new User("test@email.com", "test", "secret", "salt");
            var portfolio = new Portfolio(Guid.NewGuid(), "default");
            _userRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(user);
            _portfolioReposiotory.Setup(x => x.BrowseAsync(It.IsAny<Guid>())).ReturnsAsync(new[] { portfolio });
            _coinRepository.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(coin);
            var transactionService = new TransactionService(_transactionRepository.Object, _portfolioReposiotory.Object,
                _userRepository.Object, _coinRepository.Object, _mapper.Object);

            await transactionService.CreateAsync(Guid.NewGuid(), Guid.NewGuid(), "default", "btc", 1000M,
                DateTimeOffset.UtcNow.ToUnixTimeSeconds(), 10, "USD");

            _transactionRepository.Verify(x => x.AddAsync(It.IsAny<Transaction>()), Times.Once);
        }

        [Fact]
        public async Task Given_not_exists_user_CreateAsync_should_throw_exception()
        {
            var portfolio = new Portfolio(Guid.NewGuid(), "default");
            _userRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync((User)null);
            _portfolioReposiotory.Setup(x => x.BrowseAsync(It.IsAny<Guid>())).ReturnsAsync(new[] { portfolio });
            _coinRepository.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(coin);
            var transactionService = new TransactionService(_transactionRepository.Object, _portfolioReposiotory.Object,
                _userRepository.Object, _coinRepository.Object, _mapper.Object);

            ServiceException ex = await Should.ThrowAsync<ServiceException>(async () =>
            {
                await transactionService.CreateAsync(Guid.NewGuid(), Guid.NewGuid(), "default", "btc", 1000M,
                     DateTimeOffset.UtcNow.ToUnixTimeSeconds(), 10, "USD");
            });

            ex.Code.ShouldBe(ErrorCodes.UserNotFound);
        }

        [Fact]
        public async Task Given_not_exists_portfolio_CreateAsync_should_throw_exception()
        {
            var user = new User("test@email.com", "test", "secret", "salt");
            _userRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(user);
            _coinRepository.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(coin);
            var transactionService = new TransactionService(_transactionRepository.Object, _portfolioReposiotory.Object,
                _userRepository.Object, _coinRepository.Object, _mapper.Object);

            ServiceException ex = await Should.ThrowAsync<ServiceException>(async () =>
            {
                await transactionService.CreateAsync(Guid.NewGuid(), Guid.NewGuid(), "default", "btc", 1000M,
                     DateTimeOffset.UtcNow.ToUnixTimeSeconds(), 10, "USD");
            });

            ex.Code.ShouldBe(ErrorCodes.PortfolioNotFound);
        }

        [Fact]
        public async Task Given_not_exists_coin_CreateAsync_should_throw_exception()
        {
            var portfolio = new Portfolio(Guid.NewGuid(), "default");
            var user = new User("test@email.com", "test", "secret", "salt");
            _userRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(user);
            _portfolioReposiotory.Setup(x => x.BrowseAsync(It.IsAny<Guid>())).ReturnsAsync(new[] { portfolio });
            _coinRepository.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync((Coin)null);
            var transactionService = new TransactionService(_transactionRepository.Object, _portfolioReposiotory.Object,
                _userRepository.Object, _coinRepository.Object, _mapper.Object);

            ServiceException ex = await Should.ThrowAsync<ServiceException>(async () =>
            {
                await transactionService.CreateAsync(Guid.NewGuid(), Guid.NewGuid(), "default", "btc", 1000M,
                     DateTimeOffset.UtcNow.ToUnixTimeSeconds(), 10, "USD");
            });

            ex.Code.ShouldBe(ErrorCodes.InvalidCoin);
        }

        [Fact]
        public async Task UpdateAsync_should_invoke_UpdateAsync_on_repository()
        {
            _transactionRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(transaction);
            _coinRepository.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(coin);
            var transactionService = new TransactionService(_transactionRepository.Object, _portfolioReposiotory.Object,
                _userRepository.Object, _coinRepository.Object, _mapper.Object);

            await transactionService.UpdateAsync(Guid.NewGuid(), "btc", 100M,
                DateTimeOffset.UtcNow.ToUnixTimeSeconds(), 10, "USD");

            _transactionRepository.Verify(x => x.UpdateAsync(It.IsAny<Transaction>()), Times.Once);
        }

        [Fact]
        public async Task Given_not_exists_transaction_UpdateAsync_should_throw_exception()
        {
            _transactionRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync((Transaction)null);
            _coinRepository.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(coin);
            var transactionService = new TransactionService(_transactionRepository.Object, _portfolioReposiotory.Object,
                _userRepository.Object, _coinRepository.Object, _mapper.Object);

            ServiceException ex = await Should.ThrowAsync<ServiceException>(async () =>
            {
                await transactionService.UpdateAsync(Guid.NewGuid(), "btc", 100M,
                    DateTimeOffset.UtcNow.ToUnixTimeSeconds(), 10, "USD");
            });

            ex.Code.ShouldBe(ErrorCodes.TransactionNotFound);
        }

        [Fact]
        public async Task Given_not_exists_coin_UpdateAsync_should_throw_exception()
        {
            _transactionRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(transaction);
            _coinRepository.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync((Coin)null);
            var transactionService = new TransactionService(_transactionRepository.Object, _portfolioReposiotory.Object,
                _userRepository.Object, _coinRepository.Object, _mapper.Object);

            ServiceException ex = await Should.ThrowAsync<ServiceException>(async () =>
            {
                await transactionService.UpdateAsync(Guid.NewGuid(), "btc", 100M,
                    DateTimeOffset.UtcNow.ToUnixTimeSeconds(), 10, "USD");
            });

            ex.Code.ShouldBe(ErrorCodes.InvalidCoin);
        }

        [Fact]
        public async Task SellAsync_should_invoke_UpdateAsync_on_repository()
        {
            _transactionRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(transaction);
            var transactionService = new TransactionService(_transactionRepository.Object, _portfolioReposiotory.Object,
                _userRepository.Object, _coinRepository.Object, _mapper.Object);

            await transactionService.SellAsync(Guid.NewGuid(), 2000M);

            _transactionRepository.Verify(x => x.UpdateAsync(It.IsAny<Transaction>()), Times.Once);
        }

        [Fact]
        public async Task Given_not_exists_transaction_SellAsync_should_throw_exception()
        {
            _transactionRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync((Transaction)null);
            var transactionService = new TransactionService(_transactionRepository.Object, _portfolioReposiotory.Object,
                _userRepository.Object, _coinRepository.Object, _mapper.Object);

            ServiceException ex = await Should.ThrowAsync<ServiceException>(async () =>
            {
                await transactionService.SellAsync(Guid.NewGuid(), 2000M);
            });

            ex.Code.ShouldBe(ErrorCodes.TransactionNotFound);
        }

        [Fact]
        public async Task DeleteAsync_should_invoke_DeleteAsync_on_repository()
        {
            _transactionRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(transaction);
            var transactionService = new TransactionService(_transactionRepository.Object, _portfolioReposiotory.Object,
                _userRepository.Object, _coinRepository.Object, _mapper.Object);

            await transactionService.DeleteAsync(Guid.NewGuid());

            _transactionRepository.Verify(x => x.DeleteAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task Given_not_exists_transaction_DeleteAsync_should_throw_exception()
        {
            _transactionRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync((Transaction)null);
            var transactionService = new TransactionService(_transactionRepository.Object, _portfolioReposiotory.Object,
                _userRepository.Object, _coinRepository.Object, _mapper.Object);

            ServiceException ex = await Should.ThrowAsync<ServiceException>(async () =>
            {
                await transactionService.DeleteAsync(Guid.NewGuid());
            });

            ex.Code.ShouldBe(ErrorCodes.TransactionNotFound);
        }
    }
}