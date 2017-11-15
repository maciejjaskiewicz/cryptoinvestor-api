using CryptoInvestor.Infrastructure.Commands;
using CryptoInvestor.Infrastructure.Commands.Auth;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CryptoInvestor.Api.Controllers
{
    [Route("[controller]")]
    public abstract class ApiControllerBase : Controller
    {
        private readonly ICommandDispatcher _commandDispatecher;

        protected Guid UserId => User?.Identity?.IsAuthenticated == true ?
            Guid.Parse(User.Identity.Name) : Guid.Empty;

        public ApiControllerBase(ICommandDispatcher commandDispatcher)
        {
            _commandDispatecher = commandDispatcher;
        }

        protected async Task DispatchAsync<T>(T command) where T : ICommand
        {
            if (command is IAuthenticatedCommand authenticatedCommand)
            {
                authenticatedCommand.UserId = UserId;
            }

            await _commandDispatecher.DispatchAsync(command);
        }
    }
}