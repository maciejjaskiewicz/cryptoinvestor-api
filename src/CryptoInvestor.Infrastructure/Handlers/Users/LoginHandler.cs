using CryptoInvestor.Infrastructure.Auth;
using CryptoInvestor.Infrastructure.Commands;
using CryptoInvestor.Infrastructure.Commands.Users;
using CryptoInvestor.Infrastructure.Extensions;
using CryptoInvestor.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

namespace CryptoInvestor.Infrastructure.Handlers.Users
{
    public class LoginHandler : ICommandHandler<Login>
    {
        private readonly IUserService _userService;
        private readonly IJwtHandler _jwtHandler;
        private readonly IMemoryCache _cache;

        public LoginHandler(IUserService userService,
            IJwtHandler jwtHandler, IMemoryCache cache)
        {
            _userService = userService;
            _jwtHandler = jwtHandler;
            _cache = cache;
        }

        public async Task HandleAsync(Login command)
        {
            await _userService.LoginAsync(command.Email, command.Password);
            var user = await _userService.GetAsync(command.Email);
            var token = _jwtHandler.CreateToken(user.Id, command.Email);
            _cache.SetJwt(command.TokenId, token);
        }
    }
}