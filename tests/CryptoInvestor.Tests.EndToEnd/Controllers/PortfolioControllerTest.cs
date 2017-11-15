using CryptoInvestor.Infrastructure.Commands.Portfolio;
using CryptoInvestor.Infrastructure.DTO;
using Newtonsoft.Json;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;

namespace CryptoInvestor.Tests.EndToEnd.Controllers
{
    public class PortfolioControllerTest : AuthorizedControllerTestBase
    {
        [Fact]
        public async Task Get_should_return_portfolios_collection_for_user()
        {
            await CreateTestUser();
            var token = await AuthorizeTestUser();

            var request = new HttpRequestMessage(HttpMethod.Get, "portfolio");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await Client.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var portfolios = JsonConvert.DeserializeObject<IEnumerable<PortfolioDto>>(responseString);
            portfolios.SingleOrDefault(x => x.Name == "Default").ShouldNotBeNull();

            await DeleteTestUser();
        }

        [Fact]
        public async Task Given_valid_nameId_portfolio_should_exists()
        {
            await CreateTestUser();
            var token = await AuthorizeTestUser();

            var portfolio = await GetPortfolioAsync("default", token);
            portfolio.NameId.ShouldBe("default");

            await DeleteTestUser();
        }

        [Fact]
        public async Task Given_invalid_nameId_portfolio_should_not_exists()
        {
            await CreateTestUser();
            var token = await AuthorizeTestUser();

            var request = new HttpRequestMessage(HttpMethod.Get, "portfolio/not-exists");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await Client.SendAsync(request);

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

            await DeleteTestUser();
        }

        [Fact]
        public async Task Given_unique_portfolioName_portfolio_should_be_created()
        {
            await CreateTestUser();
            var token = await AuthorizeTestUser();

            var command = new CreatePortfolio
            {
                Name = "test"
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "portfolio");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Content = GetPayload(command);
            var response = await Client.SendAsync(request);

            response.StatusCode.ShouldBe(HttpStatusCode.Created);

            var portfolio = await GetPortfolioAsync(command.Name, token);
            portfolio.Name.ShouldBe(command.Name);

            await DeleteTestUser();
        }

        [Fact]
        public async Task Given_valid_portfolioNameId_portfolio_should_be_removed()
        {
            await CreateTestUser();
            var token = await AuthorizeTestUser();

            var request = new HttpRequestMessage(HttpMethod.Delete, "portfolio/default");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await Client.SendAsync(request);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var checkRequest = new HttpRequestMessage(HttpMethod.Get, "portfolio/default");
            checkRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var checkResponse = await Client.SendAsync(checkRequest);

            checkResponse.StatusCode.ShouldBe(HttpStatusCode.NotFound);

            await DeleteTestUser();
        }

        private async Task<PortfolioDto> GetPortfolioAsync(string nameId, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"portfolio/{nameId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await Client.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            return JsonConvert.DeserializeObject<PortfolioDto>(responseString);
        }
    }
}