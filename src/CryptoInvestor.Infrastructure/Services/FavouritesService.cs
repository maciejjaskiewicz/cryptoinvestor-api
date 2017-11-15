using AutoMapper;
using CryptoInvestor.Core.Domain;
using CryptoInvestor.Core.Repositories;
using CryptoInvestor.Infrastructure.DTO;
using CryptoInvestor.Infrastructure.Exceptions;
using CryptoInvestor.Infrastructure.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoInvestor.Infrastructure.Services
{
    public class FavouritesService : IFavouritesService
    {
        private readonly IFavouritesRepository _favouritesRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICoinRepository _coinRepository;
        private readonly IMapper _mapper;

        public FavouritesService(IFavouritesRepository favouritesRepository, IUserRepository userRepository,
            ICoinRepository coinRepository, IMapper mapper)
        {
            _favouritesRepository = favouritesRepository;
            _userRepository = userRepository;
            _coinRepository = coinRepository;
            _mapper = mapper;
        }

        public async Task<FavouritesDto> GetAsync(Guid userId)
        {
            var favourites = await _favouritesRepository.GetAsync(userId);
            favourites = await UpdateCoins(favourites);
            return _mapper.Map<FavouritesDto>(favourites);
        }

        public async Task CreateAsync(Guid userId)
        {
            var user = await _userRepository.GetAsync(userId);
            if (user == null)
            {
                throw new ServiceException(ErrorCodes.UserNotFound,
                    $"User with id: {userId} does not exists.");
            }

            var localFavourites = await _favouritesRepository.GetAsync(userId);
            if (localFavourites != null)
            {
                throw new ServiceException(ErrorCodes.FavouritesAlreadyExists,
                    $"User with id: {userId} already has a favourites collection.");
            }

            var favourites = new Favourites(user.Id);
            await _favouritesRepository.AddAsync(favourites);
        }

        public async Task<CoinDto> GetCoinAsync(Guid userId, string coinSymbol)
        {
            coinSymbol = coinSymbol.ToLowerInvariant();
            var favourites = await _favouritesRepository.GetAsync(userId);
            if (favourites == null)
            {
                throw new ServiceException(ErrorCodes.FavouritesNotFound,
                    $"Favourites collection with id: {userId} does not exists.");
            }

            var coin = favourites.Coins.SingleOrDefault(x => x.Symbol == coinSymbol);

            return _mapper.Map<CoinDto>(coin);
        }

        public async Task AddCoinAsync(Guid userId, string coinSymbol)
        {
            coinSymbol = coinSymbol.ToLowerInvariant();
            var favourites = await _favouritesRepository.GetAsync(userId);
            if (favourites == null)
            {
                throw new ServiceException(ErrorCodes.FavouritesNotFound,
                    $"Favourites collection with id: {userId} does not exists.");
            }

            var coin = await _coinRepository.GetAsync(coinSymbol);
            if (coin == null)
            {
                throw new ServiceException(ErrorCodes.InvalidCoin,
                    $"Coin with symbol: {coinSymbol} was not found.");
            }

            favourites.AddCoin(coin);
            await _favouritesRepository.UpdateAsync(favourites);
        }

        public async Task DeleteCoinAsync(Guid userId, string coinSymbol)
        {
            coinSymbol = coinSymbol.ToLowerInvariant();
            var favourites = await _favouritesRepository.GetAsync(userId);
            if (favourites == null)
            {
                throw new ServiceException(ErrorCodes.FavouritesNotFound,
                    $"Favourites collection with id: {userId} does not exists.");
            }

            var coin = await _coinRepository.GetAsync(coinSymbol);
            if (coin == null)
            {
                throw new ServiceException(ErrorCodes.InvalidCoin,
                    $"Coin with symbol: {coinSymbol} was not found.");
            }

            favourites.RemoveCoin(coin);
            await _favouritesRepository.UpdateAsync(favourites);
        }

        private async Task<Favourites> UpdateCoins(Favourites favourites)
        {
            var temp = new Favourites(favourites.Id);

            foreach (var coin in favourites.Coins)
            {
                var newCoin = await _coinRepository.GetAsync(coin.Symbol);
                temp.AddCoin(newCoin);
            }

            return temp;
        }
    }
}