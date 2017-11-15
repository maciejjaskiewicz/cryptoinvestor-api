using CryptoInvestor.Infrastructure.DTO;
using Newtonsoft.Json;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace CryptoInvestor.Tests.EndToEnd.Controllers
{
    public class CoinsControllerTest : ControllerTestBase
    {
        [Fact]
        public async Task Given_valid_coinSymbol_coin_should_exists()
        {
            var coinSymbol = "btc";

            var response = await Client.GetAsync($"coins/{coinSymbol}");
            var responseString = await response.Content.ReadAsStringAsync();

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var coin = JsonConvert.DeserializeObject<CoinDto>(responseString);
            coin.Symbol.ShouldBe(coinSymbol);
        }

        [Fact]
        public async Task Get_without_parameters_should_return_coins_collection()
        {
            var response = await Client.GetAsync($"coins");
            var responseString = await response.Content.ReadAsStringAsync();

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var coins = JsonConvert.DeserializeObject<IEnumerable<CoinDto>>(responseString);

            coins.SingleOrDefault(x => x.Symbol == "btc").ShouldNotBeNull();
        }

        [Fact]
        public async Task Get_with_short_query_parameter_should_return_coins_collection()
        {
            var response = await Client.GetAsync($"coins?short=true");
            var responseString = await response.Content.ReadAsStringAsync();

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var coins = JsonConvert.DeserializeObject<IEnumerable<CoinShortDto>>(responseString);

            coins.SingleOrDefault(x => x.Symbol == "btc").ShouldNotBeNull();
        }
    }
}