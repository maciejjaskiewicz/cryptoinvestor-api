using CryptoInvestor.Api;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace CryptoInvestor.Tests.EndToEnd.Controllers
{
    public abstract class ControllerTestBase
    {
        protected readonly TestServer Server;
        protected readonly HttpClient Client;

        protected ControllerTestBase()
        {
            Server = new TestServer(WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>()
            );

            Client = Server.CreateClient();
        }

        protected static StringContent GetPayload(object data)
        {
            var json = JsonConvert.SerializeObject(data);

            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}