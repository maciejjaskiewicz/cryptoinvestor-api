using AutoMapper;
using CryptoInvestor.Core.Domain;
using CryptoInvestor.Core.Repositories;
using CryptoInvestor.Infrastructure.DTO;
using CryptoInvestor.Infrastructure.Exceptions;
using CryptoInvestor.Infrastructure.Services;
using Moq;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CryptoInvestor.Tests.Services
{
    public class FavouritesServiceTest
    {
        private readonly Mock<IFavouritesRepository> _favouritesRepository;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<ICoinRepository> _coinRepository;
        private readonly Mock<IMapper> _mapper;

        public FavouritesServiceTest()
        {
            _favouritesRepository = new Mock<IFavouritesRepository>();
            _userRepository = new Mock<IUserRepository>();
            _coinRepository = new Mock<ICoinRepository>();
            _mapper = new Mock<IMapper>();
        }

        [Fact]
        public async Task GetAsync_should_invoke_GetAsync_on_repository()
        {
            var favourites = new Favourites(Guid.NewGuid());
            _favouritesRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(favourites);
            var favouritesService = new FavouritesService(_favouritesRepository.Object, _userRepository.Object,
                _coinRepository.Object, _mapper.Object);

            await favouritesService.GetAsync(Guid.NewGuid());

            _favouritesRepository.Verify(x => x.GetAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task GetAsync_should_update_coins_before_return()
        {
            var favourites = new Favourites(Guid.NewGuid());
            var coin = new Coin("btc", "bitcoin");
            favourites.AddCoin(coin);

            _coinRepository.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(coin);
            _favouritesRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(favourites);

            var favouritesService = new FavouritesService(_favouritesRepository.Object, _userRepository.Object,
                _coinRepository.Object, _mapper.Object);

            await favouritesService.GetAsync(Guid.NewGuid());

            _coinRepository.Verify(x => x.GetAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task GetAsync_should_return_FavouritesDto()
        {
            var favourites = new Favourites(Guid.NewGuid());
            _favouritesRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(favourites);
            _mapper.Setup(x => x.Map<FavouritesDto>(It.IsAny<Favourites>())).Returns(new FavouritesDto());
            var favouritesService = new FavouritesService(_favouritesRepository.Object, _userRepository.Object,
                _coinRepository.Object, _mapper.Object);

            var favouritesDto = await favouritesService.GetAsync(Guid.NewGuid());

            favouritesDto.ShouldBeOfType<FavouritesDto>();
        }

        [Fact]
        public async Task CreateAsync_should_invoke_AddAsync_on_repository()
        {
            var user = new User("test@email.com", "test", "secret", "salt");

            _favouritesRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync((Favourites)null);
            _userRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(user);

            var favouritesService = new FavouritesService(_favouritesRepository.Object, _userRepository.Object,
                _coinRepository.Object, _mapper.Object);

            await favouritesService.CreateAsync(Guid.NewGuid());

            _favouritesRepository.Verify(x => x.AddAsync(It.IsAny<Favourites>()), Times.Once);
        }

        [Fact]
        public async Task Given_not_exists_user_CreateAsync_should_throw_exception()
        {
            _favouritesRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync((Favourites)null);
            _userRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync((User)null);

            var favouritesService = new FavouritesService(_favouritesRepository.Object, _userRepository.Object,
                _coinRepository.Object, _mapper.Object);

            ServiceException ex = await Should.ThrowAsync<ServiceException>(async () =>
            {
                await favouritesService.CreateAsync(Guid.NewGuid());
            });

            ex.Code.ShouldBe(ErrorCodes.UserNotFound);
        }

        [Fact]
        public async Task Given_user_with_favourites_collection_CreateAsync_should_throw_exception()
        {
            var favourites = new Favourites(Guid.NewGuid());
            var user = new User("test@email.com", "test", "secret", "salt");
            _favouritesRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(favourites);
            _userRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(user);

            var favouritesService = new FavouritesService(_favouritesRepository.Object, _userRepository.Object,
                _coinRepository.Object, _mapper.Object);

            ServiceException ex = await Should.ThrowAsync<ServiceException>(async () =>
            {
                await favouritesService.CreateAsync(Guid.NewGuid());
            });

            ex.Code.ShouldBe(ErrorCodes.FavouritesAlreadyExists);
        }

        [Fact]
        public async Task GetCoinAsync_should_invoke_GetAsync_on_favourites_repository()
        {
            var favourites = new Favourites(Guid.NewGuid());
            _favouritesRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(favourites);
            var favouritesService = new FavouritesService(_favouritesRepository.Object, _userRepository.Object,
                _coinRepository.Object, _mapper.Object);

            await favouritesService.GetCoinAsync(Guid.NewGuid(), "btc");

            _favouritesRepository.Verify(x => x.GetAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task GetCoinAsync_should_return_CoinDto()
        {
            var favourites = new Favourites(Guid.NewGuid());
            _favouritesRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(favourites);
            _mapper.Setup(x => x.Map<CoinDto>(It.IsAny<Coin>())).Returns(new CoinDto());
            var favouritesService = new FavouritesService(_favouritesRepository.Object, _userRepository.Object,
                _coinRepository.Object, _mapper.Object);

            var coinDto = await favouritesService.GetCoinAsync(Guid.NewGuid(), "btc");

            coinDto.ShouldBeOfType<CoinDto>();
        }

        [Fact]
        public async Task Given_user_without_favourites_collection_GetCoinAsync_should_throw_exception()
        {
            _favouritesRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync((Favourites)null);
            var favouritesService = new FavouritesService(_favouritesRepository.Object, _userRepository.Object,
                _coinRepository.Object, _mapper.Object);

            ServiceException ex = await Should.ThrowAsync<ServiceException>(async () =>
            {
                await favouritesService.GetCoinAsync(Guid.NewGuid(), "btc");
            });

            ex.Code.ShouldBe(ErrorCodes.FavouritesNotFound);
        }

        [Fact]
        public async Task AddCoinAsync_should_invoke_UpdateAsync_on_favourites_repository()
        {
            var favourites = new Favourites(Guid.NewGuid());
            var coin = new Coin("btc", "bitcoin");
            _favouritesRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(favourites);
            _coinRepository.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(coin);

            var favouritesService = new FavouritesService(_favouritesRepository.Object, _userRepository.Object,
                _coinRepository.Object, _mapper.Object);

            await favouritesService.AddCoinAsync(Guid.NewGuid(), "btc");

            _favouritesRepository.Verify(x => x.UpdateAsync(It.IsAny<Favourites>()), Times.Once);
        }

        [Fact]
        public async Task Given_user_without_favourites_collection_AddCoinAsync_should_throw_exception()
        {
            _favouritesRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync((Favourites)null);
            var favouritesService = new FavouritesService(_favouritesRepository.Object, _userRepository.Object,
                _coinRepository.Object, _mapper.Object);

            ServiceException ex = await Should.ThrowAsync<ServiceException>(async () =>
            {
                await favouritesService.AddCoinAsync(Guid.NewGuid(), "btc");
            });

            ex.Code.ShouldBe(ErrorCodes.FavouritesNotFound);
        }

        [Fact]
        public async Task Given_favourites_collection_without_coin_AddCoinAsync_should_throw_exception()
        {
            var favourites = new Favourites(Guid.NewGuid());
            _favouritesRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(favourites);
            _coinRepository.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync((Coin)null);
            var favouritesService = new FavouritesService(_favouritesRepository.Object, _userRepository.Object,
                _coinRepository.Object, _mapper.Object);

            ServiceException ex = await Should.ThrowAsync<ServiceException>(async () =>
            {
                await favouritesService.AddCoinAsync(Guid.NewGuid(), "btc");
            });

            ex.Code.ShouldBe(ErrorCodes.InvalidCoin);
        }

        [Fact]
        public async Task DeleteCoinAsync_should_invoke_UpdateAsync_on_favourites_repository()
        {
            var favourites = new Favourites(Guid.NewGuid());
            var coin = new Coin("btc", "bitcoin");
            favourites.AddCoin(coin);
            _favouritesRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(favourites);
            _coinRepository.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(coin);

            var favouritesService = new FavouritesService(_favouritesRepository.Object, _userRepository.Object,
                _coinRepository.Object, _mapper.Object);

            await favouritesService.DeleteCoinAsync(Guid.NewGuid(), "btc");

            _favouritesRepository.Verify(x => x.UpdateAsync(It.IsAny<Favourites>()), Times.Once);
        }

        [Fact]
        public async Task Given_user_without_favourites_collection_DeleteCoinAsync_should_throw_exception()
        {
            _favouritesRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync((Favourites)null);
            var favouritesService = new FavouritesService(_favouritesRepository.Object, _userRepository.Object,
                _coinRepository.Object, _mapper.Object);

            ServiceException ex = await Should.ThrowAsync<ServiceException>(async () =>
            {
                await favouritesService.DeleteCoinAsync(Guid.NewGuid(), "btc");
            });

            ex.Code.ShouldBe(ErrorCodes.FavouritesNotFound);
        }

        [Fact]
        public async Task Given_favourites_collection_without_coin_DeleteCoinAsync_should_throw_exception()
        {
            var favourites = new Favourites(Guid.NewGuid());
            _favouritesRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(favourites);
            _coinRepository.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync((Coin)null);
            var favouritesService = new FavouritesService(_favouritesRepository.Object, _userRepository.Object,
                _coinRepository.Object, _mapper.Object);

            ServiceException ex = await Should.ThrowAsync<ServiceException>(async () =>
            {
                await favouritesService.DeleteCoinAsync(Guid.NewGuid(), "btc");
            });

            ex.Code.ShouldBe(ErrorCodes.InvalidCoin);
        }
    }
}