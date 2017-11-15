using CryptoInvestor.Infrastructure.DTO;
using Newtonsoft.Json;
using Shouldly;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace CryptoInvestor.Tests.EndToEnd.Controllers
{
    public class UsersControllerTest : ControllerTestBase
    {
        [Fact]
        public async Task Given_valid_email_user_should_exist()
        {
            var email = "user1@email.com";
            var user = await GetUserAsync(email);

            user.Email.ShouldBe(email);
        }

        [Fact]
        public async Task Given_invalid_email_user_should_not_exist()
        {
            var email = "user1000@email.com";
            var response = await Client.GetAsync($"users/{email}");

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        private async Task<UserDto> GetUserAsync(string email)
        {
            var response = await Client.GetAsync($"users/{email}");
            var responseString = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<UserDto>(responseString);
        }
    }
}