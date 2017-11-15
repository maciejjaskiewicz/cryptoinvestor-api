using CryptoInvestor.Infrastructure.Commands.Users;
using CryptoInvestor.Infrastructure.DTO;
using Newtonsoft.Json;
using Shouldly;
using System;
using System.Net;
using System.Threading.Tasks;

namespace CryptoInvestor.Tests.EndToEnd.Controllers
{
    public abstract class AuthorizedControllerTestBase : ControllerTestBase
    {
        private Random random = new Random();
        protected string TestUserEmail = "test123@email.com";

        protected async Task CreateTestUser()
        {
            TestUserEmail = $"test{random.Next(1, 1000)}@email.com";
            var command = new CreateUser
            {
                Email = TestUserEmail,
                Password = "secret123",
                Username = "test"
            };
            var payload = GetPayload(command);

            var response = await Client.PostAsync("account/register", payload);

            response.StatusCode.ShouldBe(HttpStatusCode.Created);
        }

        protected async Task DeleteTestUser()
        {
            var user = await GetUserAsync(TestUserEmail);

            await DeleteUser(user.Id);
        }

        protected async Task<string> AuthorizeTestUser()
        {
            var command = new Login
            {
                Email = TestUserEmail,
                Password = "secret123"
            };

            var token = await LoginAsync(command);
            token.ShouldNotBeNullOrEmpty();

            return token;
        }

        protected async Task<string> LoginAsync(Login command)
        {
            var payload = GetPayload(command);
            var response = await Client.PostAsync("account/login", payload);

            response.EnsureSuccessStatusCode();

            return GetToken(await response.Content.ReadAsStringAsync());
        }

        protected async Task<UserDetailsDto> GetUserAsync(string email)
        {
            var response = await Client.GetAsync($"users/{email}");
            var responseString = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<UserDetailsDto>(responseString);
        }

        protected async Task DeleteUser(Guid id)
        {
            var response = await Client.DeleteAsync($"users/{id}");

            response.EnsureSuccessStatusCode();
        }

        protected string GetToken(string responseString)
        {
            dynamic json = JsonConvert.DeserializeObject<dynamic>(responseString);
            return json.token;
        }
    }
}