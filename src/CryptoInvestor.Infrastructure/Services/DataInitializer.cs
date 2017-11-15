using CryptoInvestor.Core.Domain;
using CryptoInvestor.Core.Repositories;
using CryptoInvestor.Infrastructure.Services.Interfaces;
using Newtonsoft.Json;
using NLog;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CryptoInvestor.Infrastructure.Services
{
    public class DataInitializer : IDataInitializer
    {
        private readonly IUserService _userService;
        private readonly IPortfolioService _portfolioService;
        private readonly IFavouritesService _favouritesService;
        private readonly ICoinRepository _coinRepository;
        private readonly HttpClient _client;

        private static readonly Random random = new Random();
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly string api1Url = "https://api.coinmarketcap.com/v1/ticker/?limit=1000";
        private readonly string api2Url = "https://min-api.cryptocompare.com/data/histohour";
        private readonly string iconsApiUrl = "http://files.jaskiewicz.tech/cryptoinvestor/coins/32x32";

        public DataInitializer(IUserService userService, IPortfolioService portfolioService,
            ICoinRepository coinRepository, IFavouritesService favouritesService)
        {
            _userService = userService;
            _portfolioService = portfolioService;
            _coinRepository = coinRepository;
            _favouritesService = favouritesService;
            _client = new HttpClient();
        }

        public async Task SeedAsync()
        {
            var users = await _userService.BrowseAsync();
            var coins = await _coinRepository.BrowseAsync();

            if (!coins.Any())
            {
                await InitializeCoins();
            }

            if (!users.Any())
            {
                await InitializeUsers(10);
            }
        }

        private async Task InitializeUsers(int numberOfUsers)
        {
            Logger.Info("Initializing users...");

            for (int i = 1; i <= numberOfUsers; i++)
            {
                var username = $"user{i}";
                await _userService.RegisterAsync($"{username}@email.com", username, "secret123");
                var user = await _userService.GetAsync($"{username}@email.com");

                await _favouritesService.CreateAsync(user.Id);
                await _favouritesService.AddCoinAsync(user.Id, "BTC");
                await _portfolioService.CreateAsync(user.Id, "Default");
            }

            Logger.Info("Users initialized.");
        }

        private async Task InitializeCoins()
        {
            Logger.Info("Initializing coins...");

            var response = await _client.GetAsync(api1Url);
            var responseString = await response.Content.ReadAsStringAsync();
            dynamic json = JsonConvert.DeserializeObject<dynamic>(responseString);

            foreach (dynamic coin in json)
            {
                string symbol = coin.symbol;
                var result = await CheckIsValid(symbol);
                if (result)
                {
                    SaveCoin(coin);
                }
            }

            Logger.Info("Coins initialized.");
        }

        private async Task<bool> CheckIsValid(string symbol)
        {
            var url = $"{api2Url}?fsym={symbol}&tsym=USD&limit=1&aggregate=1&e=CCCAGG";
            var response = await _client.GetAsync(url);
            var responseString = await response.Content.ReadAsStringAsync();
            dynamic json = JsonConvert.DeserializeObject<dynamic>(responseString);

            if (json.Response == "Error")
            {
                return false;
            }

            return true;
        }

        private async Task SaveCoin(dynamic coin)
        {
            string symbol = coin.symbol;
            var localCoin = await _coinRepository.GetAsync(symbol.ToLowerInvariant());
            if (localCoin != null) return;

            string name = coin.name;
            decimal priceUsd = coin.price_usd;
            decimal marketCap = coin.market_cap_usd;
            decimal change24h = coin.percent_change_24h;
            decimal volume24h = coin["24h_volume_usd"];

            symbol = symbol.ToLowerInvariant();

            var newCoin = new Coin(symbol, name);
            newCoin.AddPrice(CoinPrice.Create("USD", priceUsd));
            newCoin.SetMarketCap(marketCap);
            newCoin.SetChange24h(change24h);
            newCoin.SetVolume24h(volume24h);
            newCoin.SetIconUrl($"{iconsApiUrl}/{symbol}.png");

            await _coinRepository.AddAsync(newCoin);
        }
    }
}