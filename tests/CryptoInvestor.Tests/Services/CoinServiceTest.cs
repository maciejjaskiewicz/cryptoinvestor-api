using AutoMapper;
using CryptoInvestor.Core.Domain;
using CryptoInvestor.Core.Repositories;
using CryptoInvestor.Infrastructure.DTO;
using CryptoInvestor.Infrastructure.Services;
using Moq;
using Shouldly;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CryptoInvestor.Tests.Services
{
    public class CoinServiceTest
    {
        private readonly Mock<ICoinRepository> _coinRepository;
        private readonly Mock<IMapper> _mapper;

        public CoinServiceTest()
        {
            _coinRepository = new Mock<ICoinRepository>();
            _mapper = new Mock<IMapper>();
        }

        [Fact]
        public async Task GetAsync_should_invoke_GetAsync_on_repository()
        {
            var coin = new Coin("btc", "bitcoin");
            _coinRepository.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(coin);
            var coinService = new CoinService(_coinRepository.Object, _mapper.Object);

            await coinService.GetAsync("btc");

            _coinRepository.Verify(x => x.GetAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task BrowseAsync_should_invoke_BrowseAsync_on_repository()
        {
            var coins = GetCoinsCollection();
            _coinRepository.Setup(x => x.BrowseAsync()).ReturnsAsync(coins);
            var coinService = new CoinService(_coinRepository.Object, _mapper.Object);

            await coinService.BrowseAsync();

            _coinRepository.Verify(x => x.BrowseAsync(), Times.Once);
        }

        [Fact]
        public async Task BrowseAsync_should_return_IEnumerable_collection_of_CoinDto()
        {
            var coins = GetCoinsCollection();
            _coinRepository.Setup(x => x.BrowseAsync()).ReturnsAsync(coins);
            _mapper.Setup(x => x.Map<IEnumerable<CoinDto>>(It.IsAny<Coin>())).Returns(new[] { new CoinDto() });
            var coinService = new CoinService(_coinRepository.Object, _mapper.Object);

            var coinDtos = await coinService.BrowseAsync();

            coinDtos.ShouldBeAssignableTo<IEnumerable<CoinDto>>();
        }

        [Fact]
        public async Task BrowseShortAsync_should_invoke_BrowseAsync_on_repository()
        {
            var coins = GetCoinsCollection();
            _coinRepository.Setup(x => x.BrowseAsync()).ReturnsAsync(coins);
            var coinService = new CoinService(_coinRepository.Object, _mapper.Object);

            await coinService.BrowseShortAsync();

            _coinRepository.Verify(x => x.BrowseAsync(), Times.Once);
        }

        [Fact]
        public async Task BrowseShortAsync_should_return_IEnumerable_collection_of_CoinShortDto()
        {
            var coins = GetCoinsCollection();
            _coinRepository.Setup(x => x.BrowseAsync()).ReturnsAsync(coins);
            _mapper.Setup(x => x.Map<IEnumerable<CoinShortDto>>(It.IsAny<Coin>())).Returns(new[] { new CoinShortDto() });
            var coinService = new CoinService(_coinRepository.Object, _mapper.Object);

            var coinDtos = await coinService.BrowseShortAsync();

            coinDtos.ShouldBeAssignableTo<IEnumerable<CoinShortDto>>();
        }

        private IEnumerable<Coin> GetCoinsCollection()
        {
            yield return new Coin("btc", "bitcoin");
            yield return new Coin("eth", "etherium");
        }
    }
}