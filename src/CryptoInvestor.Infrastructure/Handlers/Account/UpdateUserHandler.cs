using CryptoInvestor.Infrastructure.Commands;
using CryptoInvestor.Infrastructure.Commands.Account;
using CryptoInvestor.Infrastructure.Services.Interfaces;
using System.Threading.Tasks;

namespace CryptoInvestor.Infrastructure.Handlers.Account
{
    public class UpdateUserHandler : ICommandHandler<UpdateUser>
    {
        private readonly IUserService _userService;

        public UpdateUserHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task HandleAsync(UpdateUser command)
        {
            var user = await _userService.GetAsync(command.UserId);
            await _userService.UpdateAsync(user.Email, command.FirstName, command.LastName, command.Gender);
        }
    }
}