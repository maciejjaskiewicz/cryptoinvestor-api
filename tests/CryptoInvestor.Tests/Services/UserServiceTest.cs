using AutoMapper;
using CryptoInvestor.Core.Domain;
using CryptoInvestor.Core.Repositories;
using CryptoInvestor.Infrastructure.Auth;
using CryptoInvestor.Infrastructure.DTO;
using CryptoInvestor.Infrastructure.Exceptions;
using CryptoInvestor.Infrastructure.Services;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CryptoInvestor.Tests.Services
{
    public class UserServiceTest
    {
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IEncrypter> _encrypter;
        private readonly Mock<IMapper> _mapper;

        public UserServiceTest()
        {
            _userRepository = new Mock<IUserRepository>();
            _encrypter = new Mock<IEncrypter>();
            _mapper = new Mock<IMapper>();

            _encrypter.Setup(x => x.GetSalt(It.IsAny<string>())).Returns("salt");
            _encrypter.Setup(x => x.GetHash(It.IsAny<string>(), It.IsAny<string>())).Returns("hash");
        }

        [Fact]
        public async Task GetAsync_by_email_should_invoke_GetAsync_on_repository()
        {
            var user = new User("test@email.com", "test", "secret", "salt");
            _userRepository.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(user);
            var userService = new UserService(_userRepository.Object, _encrypter.Object, _mapper.Object);

            await userService.GetAsync("test@email.com");

            _userRepository.Verify(x => x.GetAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task GetAsync_by_email_should_return_UserDetailsDto()
        {
            var user = new User("test@email.com", "test", "secret", "salt");
            _userRepository.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(user);
            _mapper.Setup(x => x.Map<UserDetailsDto>(It.IsAny<User>())).Returns(new UserDetailsDto());
            var userService = new UserService(_userRepository.Object, _encrypter.Object, _mapper.Object);

            var userDto = await userService.GetAsync("test@email.com");

            userDto.ShouldBeOfType<UserDetailsDto>();
        }

        [Fact]
        public async Task GetAsync_by_id_should_invoke_GetAsync_on_repository()
        {
            var user = new User("test@email.com", "test", "secret", "salt");
            _userRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(user);
            var userService = new UserService(_userRepository.Object, _encrypter.Object, _mapper.Object);

            await userService.GetAsync(Guid.NewGuid());

            _userRepository.Verify(x => x.GetAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task GetAsync_by_id_should_return_UserDetailsDto()
        {
            var user = new User("test@email.com", "test", "secret", "salt");
            _userRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(user);
            _mapper.Setup(x => x.Map<UserDetailsDto>(It.IsAny<User>())).Returns(new UserDetailsDto());
            var userService = new UserService(_userRepository.Object, _encrypter.Object, _mapper.Object);

            var userDto = await userService.GetAsync(Guid.NewGuid());

            userDto.ShouldBeOfType<UserDetailsDto>();
        }

        [Fact]
        public async Task BrowseAsync_should_invoke_BrowseAsync_on_repository()
        {
            var users = GetUsersCollection();
            _userRepository.Setup(x => x.BrowseAsync()).ReturnsAsync(users);
            var userService = new UserService(_userRepository.Object, _encrypter.Object, _mapper.Object);

            await userService.BrowseAsync();

            _userRepository.Verify(x => x.BrowseAsync(), Times.Once);
        }

        [Fact]
        public async Task BrowseAsync_should_return_IEnumerable_collection_of_UserDto()
        {
            var users = GetUsersCollection();
            _userRepository.Setup(x => x.BrowseAsync()).ReturnsAsync(users);
            _mapper.Setup(x => x.Map<IEnumerable<UserDto>>(It.IsAny<IEnumerable<User>>())).Returns(new[] { new UserDto() });
            var userService = new UserService(_userRepository.Object, _encrypter.Object, _mapper.Object);

            var userDtos = await userService.BrowseAsync();

            userDtos.ShouldBeAssignableTo<IEnumerable<UserDto>>();
        }

        [Fact]
        public void Given_valid_credentials_LoginAsync_should_not_throw_any_exceptions()
        {
            var user = new User("test@email.com", "test", "hash", "salt");
            _userRepository.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(user);
            var userService = new UserService(_userRepository.Object, _encrypter.Object, _mapper.Object);

            Should.NotThrow(async () =>
            {
                await userService.LoginAsync(user.Email, user.Password);
            });
        }

        [Fact]
        public async Task Given_not_exists_email_LoginAsync_should_throw_exception()
        {
            var user = new User("test@email.com", "test", "hash", "salt");
            _userRepository.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync((User)null);
            var userService = new UserService(_userRepository.Object, _encrypter.Object, _mapper.Object);

            ServiceException ex = await Should.ThrowAsync<ServiceException>(async () =>
            {
                await userService.LoginAsync(user.Email, user.Password);
            });

            ex.Code.ShouldBe(ErrorCodes.InvalidCredentials);
        }

        [Fact]
        public async Task Given_invalid_password_LoginAsync_should_throw_exception()
        {
            var user = new User("test@email.com", "test", "invalid_password", "salt");
            _userRepository.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(user);
            var userService = new UserService(_userRepository.Object, _encrypter.Object, _mapper.Object);

            ServiceException ex = await Should.ThrowAsync<ServiceException>(async () =>
            {
                await userService.LoginAsync(user.Email, user.Password);
            });

            ex.Code.ShouldBe(ErrorCodes.InvalidCredentials);
        }

        [Fact]
        public async Task RegisteAsync_should_invoke_AddAsync_on_repository()
        {
            var userService = new UserService(_userRepository.Object, _encrypter.Object, _mapper.Object);
            await userService.RegisterAsync("test@email.com", "test", "secret");

            _userRepository.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task Given_exists_email_RegisterAsync_should_throw_exception()
        {
            var user = new User("test@email.com", "test", "secret", "salt");
            _userRepository.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(user);
            var userService = new UserService(_userRepository.Object, _encrypter.Object, _mapper.Object);

            ServiceException ex = await Should.ThrowAsync<ServiceException>(async () =>
            {
                await userService.RegisterAsync("test@email.com", "test", "secret");
            });

            ex.Code.ShouldBe(ErrorCodes.EmailInUse);
        }

        [Fact]
        public async Task UpdateAsync_should_invoke_UpdateAsync_on_repository()
        {
            var user = new User("test@email.com", "test", "secret", "salt");
            _userRepository.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(user);
            var userService = new UserService(_userRepository.Object, _encrypter.Object, _mapper.Object);

            await userService.UpdateAsync("test@email.com", "Jan", "Kowalski", "male");

            _userRepository.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task Given_not_exists_email_UpdateAsync_should_throw_exception()
        {
            var user = new User("test@email.com", "test", "secret", "salt");
            _userRepository.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync((User)null);
            var userService = new UserService(_userRepository.Object, _encrypter.Object, _mapper.Object);

            ServiceException ex = await Should.ThrowAsync<ServiceException>(async () =>
            {
                await userService.UpdateAsync("test@email.com", "Jan", "Kowalski", "male");
            });

            ex.Code.ShouldBe(ErrorCodes.UserNotFound);
        }

        [Fact]
        public async Task DeleteAsync_should_invoke_DeleteAsync_on_repository()
        {
            var user = new User("test@email.com", "test", "secret", "salt");
            _userRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(user);
            var userService = new UserService(_userRepository.Object, _encrypter.Object, _mapper.Object);

            await userService.DeleteAsync(Guid.NewGuid());

            _userRepository.Verify(x => x.DeleteAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task Given_not_exists_user_id_DeleteAsync_should_throw_exception()
        {
            var user = new User("test@email.com", "test", "secret", "salt");
            _userRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync((User)null);
            var userService = new UserService(_userRepository.Object, _encrypter.Object, _mapper.Object);

            ServiceException ex = await Should.ThrowAsync<ServiceException>(async () =>
            {
                await userService.DeleteAsync(Guid.NewGuid());
            });

            ex.Code.ShouldBe(ErrorCodes.UserNotFound);
        }

        private IEnumerable<User> GetUsersCollection()
        {
            for (int i = 1; i <= 10; i++)
            {
                yield return new User($"test{i}@email.com", $"test{i}", "hash", "salt");
            }
        }
    }
}