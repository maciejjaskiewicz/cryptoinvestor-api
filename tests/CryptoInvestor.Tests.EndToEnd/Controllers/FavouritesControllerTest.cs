using CryptoInvestor.Infrastructure.Commands.Favourites;
using CryptoInvestor.Infrastructure.DTO;
using Newtonsoft.Json;
using Shouldly;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;

namespace CryptoInvestor.Tests.EndToEnd.Controllers
{
    public class FavouritesControllerTest : AuthorizedControllerTestBase
    {
        [Fact]
        public async Task Get_should_return_favourites_for_user()
        {
            await CreateTestUser();
            var token = await AuthorizeTestUser();

            var favourites = await GetFavourites(token);

            favourites.ShouldNotBeNull();

            await DeleteTestUser();
        }

        [Fact]
        public async Task Given_valid_data_coin_should_be_added_to_favourites()
        {
            await CreateTestUser();
            var token = await AuthorizeTestUser();

            await AddCoinToFavourites("btc", token);

            var favourites = await GetFavourites(token);
            favourites.Coins.SingleOrDefault(x => x.Symbol == "btc").ShouldNotBeNull();

            await DeleteTestUser();
        }

        [Fact]
        public async Task Given_valid_coinSymbol_coin_should_exists()
        {
            await CreateTestUser();
            var token = await AuthorizeTestUser();
            await AddCoinToFavourites("btc", token);

            var request = new HttpRequestMessage(HttpMethod.Get, "favourites/btc");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await Client.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var coin = JsonConvert.DeserializeObject<CoinDto>(responseString);
            coin.Symbol.ShouldBe("btc");

            await DeleteTestUser();
        }

        [Fact]
        public async Task Given_invalid_coinSymbol_coin_should_not_exists()
        {
            await CreateTestUser();
            var token = await AuthorizeTestUser();

            var request = new HttpRequestMessage(HttpMethod.Get, "favourites/btc");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await Client.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

            await DeleteTestUser();
        }

        [Fact]
        public async Task Given_valid_coinSymbol_coin_should_be_removed()
        {
            await CreateTestUser();
            var token = await AuthorizeTestUser();
            await AddCoinToFavourites("btc", token);

            var request = new HttpRequestMessage(HttpMethod.Delete, "favourites/btc");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await Client.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var favourites = await GetFavourites(token);
            favourites.Coins.SingleOrDefault(x => x.Symbol == "btc").ShouldBeNull();

            await DeleteTestUser();
        }

        private async Task<FavouritesDto> GetFavourites(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "favourites");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await Client.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            return JsonConvert.DeserializeObject<FavouritesDto>(responseString);
        }

        private async Task AddCoinToFavourites(string coinSymbol, string token)
        {
            var command = new AddCoinToFavourites
            {
                CoinSymbol = coinSymbol
            };

            var request = new HttpRequestMessage(HttpMethod.Put, "favourites");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Content = GetPayload(command);
            var response = await Client.SendAsync(request);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }
    }
}