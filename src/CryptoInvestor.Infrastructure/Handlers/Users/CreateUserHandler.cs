using CryptoInvestor.Infrastructure.Commands;
using CryptoInvestor.Infrastructure.Commands.Users;
using CryptoInvestor.Infrastructure.Services.Interfaces;
using System.Threading.Tasks;

namespace CryptoInvestor.Infrastructure.Handlers.Users
{
    public class CreateUserHandler : ICommandHandler<CreateUser>
    {
        private readonly IUserService _userService;
        private readonly IFavouritesService _favouritesService;
        private readonly IPortfolioService _portfolioService;

        public CreateUserHandler(IUserService userService, IFavouritesService favouritesService,
            IPortfolioService portfolioService)
        {
            _userService = userService;
            _favouritesService = favouritesService;
            _portfolioService = portfolioService;
        }

        public async Task HandleAsync(CreateUser command)
        {
            await _userService.RegisterAsync(command.Email, command.Username, command.Password);
            var createdUser = await _userService.GetAsync(command.Email);

            await _favouritesService.CreateAsync(createdUser.Id);
            await _portfolioService.CreateAsync(createdUser.Id, "Default");
        }
    }
}