using CryptoInvestor.Infrastructure.Commands.Account;
using CryptoInvestor.Infrastructure.Commands.Users;
using CryptoInvestor.Infrastructure.DTO;
using Newtonsoft.Json;
using Shouldly;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;

namespace CryptoInvestor.Tests.EndToEnd.Controllers
{
    public class AccountControllerTest : AuthorizedControllerTestBase
    {
        [Fact]
        public async Task Given_unique_email_user_should_be_created()
        {
            var command = new CreateUser
            {
                Email = "test@email.com",
                Password = "secret",
                Username = "test"
            };
            var payload = GetPayload(command);

            var response = await Client.PostAsync("account/register", payload);

            response.StatusCode.ShouldBe(HttpStatusCode.Created);
            response.Headers.Location.ToString().ShouldBe($"users/{command.Email}");

            var user = await GetUserAsync(command.Email);
            user.Email.ShouldBe(command.Email);

            await DeleteUser(user.Id);
        }

        [Fact]
        public async Task Given_valid_credentials_token_should_be_returned()
        {
            var command = new Login
            {
                Email = "user1@email.com",
                Password = "secret123"
            };

            var token = await LoginAsync(command);
            token.ShouldNotBeNullOrEmpty();
        }

        [Fact]
        public async Task Given_valid_credentials_and_token_user_should_be_authorized()
        {
            var command = new Login
            {
                Email = "user1@email.com",
                Password = "secret123"
            };

            var token = await LoginAsync(command);
            token.ShouldNotBeNullOrEmpty();

            var request = new HttpRequestMessage(HttpMethod.Get, "account/me");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await Client.SendAsync(request);
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var responseString = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<UserDetailsDto>(responseString);

            user.Email.ShouldBe(command.Email);
        }

        [Fact]
        public async Task Given_valid_data_user_should_be_updated()
        {
            await CreateTestUser();
            var token = await AuthorizeTestUser();

            var command = new UpdateUser
            {
                FirstName = "Jan",
                LastName = "Kowalski",
                Gender = "male"
            };

            var request = new HttpRequestMessage(HttpMethod.Put, "account/me");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Content = GetPayload(command);
            var response = await Client.SendAsync(request);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var user = await GetUserAsync(TestUserEmail);

            user.FirstName.ShouldBe(command.FirstName);
            user.LastName.ShouldBe(command.LastName);
            user.Gender.ShouldBe(command.Gender);

            await DeleteTestUser();
        }
    }
}