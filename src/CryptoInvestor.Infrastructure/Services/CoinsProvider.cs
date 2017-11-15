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
    public class CoinsProvider : ICoinsProvider
    {
        private readonly ICoinRepository _coinRepository;
        private readonly HttpClient _client;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly string apiUrl = "https://api.coinmarketcap.com/v1/ticker/?limit=1000&convert=PLN";

        public CoinsProvider(ICoinRepository coinRepository)
        {
            _coinRepository = coinRepository;
            _client = new HttpClient();
        }

        public async Task Provide()
        {
            var coins = await _coinRepository.BrowseAsync();

            if (!coins.Any())
            {
                return;
            }

            await UpdateCoinsCollection();
        }

        private async Task UpdateCoinsCollection()
        {
            Logger.Info("Updating coins collection.");

            var response = await _client.GetAsync(apiUrl);
            var responseString = await response.Content.ReadAsStringAsync();
            dynamic json = JsonConvert.DeserializeObject<dynamic>(responseString);

            if (!response.IsSuccessStatusCode) return;

            foreach (dynamic coin in json)
            {
                try
                {
                    await UpdateCoin(coin);
                }
                catch (Exception)
                {
                    Logger.Warn($"Coin parse error: {coin.symbol}");
                }
            }

            Logger.Info("Coins collection updated.");
        }

        private async Task UpdateCoin(dynamic coin)
        {
            string symbol = coin.symbol;
            var localCoin = await _coinRepository.GetAsync(symbol.ToLowerInvariant());
            if (localCoin == null) return;

            string name = coin.name;
            decimal priceUsd = coin.price_usd;
            decimal pricePln = coin.price_pln;
            decimal marketCap = coin.market_cap_usd;
            decimal change24h = coin.percent_change_24h;
            decimal volume24h = coin["24h_volume_usd"];

            localCoin.ClearPrices();
            localCoin.AddPrice(CoinPrice.Create("USD", priceUsd));
            localCoin.AddPrice(CoinPrice.Create("PLN", pricePln));
            localCoin.SetMarketCap(marketCap);
            localCoin.SetChange24h(change24h);
            localCoin.SetVolume24h(volume24h);

            await _coinRepository.UpdateAsync(localCoin);
        }
    }
}