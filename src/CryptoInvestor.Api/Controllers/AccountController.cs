using CryptoInvestor.Infrastructure.Commands;
using CryptoInvestor.Infrastructure.Commands.Account;
using CryptoInvestor.Infrastructure.Commands.Users;
using CryptoInvestor.Infrastructure.Extensions;
using CryptoInvestor.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace CryptoInvestor.Api.Controllers
{
    public class AccountController : ApiControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMemoryCache _cache;

        public AccountController(ICommandDispatcher commandDispatcher,
            IUserService userService, IMemoryCache cache) : base(commandDispatcher)
        {
            _userService = userService;
            _cache = cache;
        }

        [HttpGet]
        [Authorize]
        [Route("me")]
        public async Task<IActionResult> Get()
        {
            var user = await _userService.GetAsync(UserId);
            return Json(user);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Post([FromBody]Login command)
        {
            command.TokenId = Guid.NewGuid();
            await DispatchAsync(command);
            var token = _cache.GetJwt(command.TokenId);

            return Json(token);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Post([FromBody]CreateUser command)
        {
            await DispatchAsync(command);

            return Created($"users/{command.Email}", null);
        }

        [HttpPut]
        [Authorize]
        [Route("me")]
        public async Task<IActionResult> Put([FromBody]UpdateUser command)
        {
            await DispatchAsync(command);

            return Ok();
        }
    }
}