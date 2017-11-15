using CryptoInvestor.Infrastructure.Commands.Transaction;
using CryptoInvestor.Infrastructure.DTO;
using Newtonsoft.Json;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;

namespace CryptoInvestor.Tests.EndToEnd.Controllers
{
    public class TransactionsControllerTest : AuthorizedControllerTestBase
    {
        [Fact]
        public async Task Get_should_return_transactions_collection_for_user()
        {
            await CreateTestUser();
            var token = await AuthorizeTestUser();

            var request = new HttpRequestMessage(HttpMethod.Get, "transactions");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await Client.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var transactions = JsonConvert.DeserializeObject<IEnumerable<TransactionDto>>(responseString);
            transactions.ShouldNotBeNull();

            await DeleteTestUser();
        }

        [Fact]
        public async Task Given_valid_id_transaction_should_exists()
        {
            await CreateTestUser();
            var token = await AuthorizeTestUser();
            var id = await CreateTransactionAsync(token);

            var request = new HttpRequestMessage(HttpMethod.Get, $"transactions/{id}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await Client.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var transaction = JsonConvert.DeserializeObject<TransactionDto>(responseString);
            transaction.ShouldNotBeNull();

            await DeleteTestUser();
        }

        [Fact]
        public async Task Given_invalid_id_transaction_should_not_exists()
        {
            await CreateTestUser();
            var token = await AuthorizeTestUser();

            var request = new HttpRequestMessage(HttpMethod.Get, $"transactions/{Guid.NewGuid()}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await Client.SendAsync(request);

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

            await DeleteTestUser();
        }

        [Fact]
        public async Task Given_valid_data_tranasction_should_be_updated()
        {
            await CreateTestUser();
            var token = await AuthorizeTestUser();
            var id = await CreateTransactionAsync(token);

            var command = new UpdateTransaction
            {
                Id = id,
                CoinSymbol = "btc",
                PurchasePrice = 1000M,
                PurchaseDate = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                Amount = 10,
                Sold = true,
                SoldPrice = 2000M,
                Currency = "USD"
            };

            var request = new HttpRequestMessage(HttpMethod.Put, "transactions");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Content = GetPayload(command);
            var response = await Client.SendAsync(request);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var transaction = await GetTransactionAsync(id, token);
            transaction.IsSold.ShouldBe(true);

            await DeleteTestUser();
        }

        [Fact]
        public async Task Given_valid_id_transaction_should_be_removed()
        {
            await CreateTestUser();
            var token = await AuthorizeTestUser();
            var id = await CreateTransactionAsync(token);

            var request = new HttpRequestMessage(HttpMethod.Delete, $"transactions/{id}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await Client.SendAsync(request);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var checkRequest = new HttpRequestMessage(HttpMethod.Get, $"transactions/{id}");
            checkRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var checkResponse = await Client.SendAsync(checkRequest);

            checkResponse.StatusCode.ShouldBe(HttpStatusCode.NotFound);

            await DeleteTestUser();
        }

        private async Task<Guid> CreateTransactionAsync(string token)
        {
            var command = new CreateTransaction
            {
                PortfolioNameId = "default",
                CoinSymbol = "btc",
                PurchasePrice = 1000M,
                PurchaseDate = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                Amount = 10,
                Currency = "USD"
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "transactions");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Content = GetPayload(command);
            var response = await Client.SendAsync(request);

            response.StatusCode.ShouldBe(HttpStatusCode.Created);
            var id = response.Headers.Location.ToString().Split('/')[1];

            return Guid.Parse(id);
        }

        private async Task<TransactionDto> GetTransactionAsync(Guid id, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"transactions/{id}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await Client.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            return JsonConvert.DeserializeObject<TransactionDto>(responseString);
        }
    }
}