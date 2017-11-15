using AutoMapper;
using CryptoInvestor.Core.Domain;
using CryptoInvestor.Core.Repositories;
using CryptoInvestor.Infrastructure.Auth;
using CryptoInvestor.Infrastructure.DTO;
using CryptoInvestor.Infrastructure.Exceptions;
using CryptoInvestor.Infrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoInvestor.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEncrypter _encrypter;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository,
            IEncrypter encrypter, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _encrypter = encrypter;
        }

        public async Task<UserDetailsDto> GetAsync(string email)
        {
            var user = await _userRepository.GetAsync(email);
            return _mapper.Map<UserDetailsDto>(user);
        }

        public async Task<UserDetailsDto> GetAsync(Guid id)
        {
            var user = await _userRepository.GetAsync(id);
            return _mapper.Map<UserDetailsDto>(user);
        }

        public async Task<IEnumerable<UserDto>> BrowseAsync()
        {
            var users = await _userRepository.BrowseAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetAsync(email);
            if (user == null)
            {
                throw new ServiceException(ErrorCodes.InvalidCredentials, "Invalid credentials.");
            }

            var hash = _encrypter.GetHash(password, user.Salt);

            if (user.Password == hash)
            {
                return;
            }
            throw new ServiceException(ErrorCodes.InvalidCredentials, "Invalid credentials.");
        }

        public async Task RegisterAsync(string email, string username, string password)
        {
            if (await EmailInUseAsync(email))
            {
                throw new ServiceException(ErrorCodes.EmailInUse, $"User with email: {email} already exists.");
            }

            var salt = _encrypter.GetSalt(password);
            var hash = _encrypter.GetHash(password, salt);
            var user = new User(email, username, hash, salt);

            await _userRepository.AddAsync(user);
        }

        public async Task UpdateAsync(string email, string firstName, string lastName, string gender)
        {
            var user = await _userRepository.GetAsync(email);
            if (user == null)
            {
                throw new ServiceException(ErrorCodes.UserNotFound, $"User with email: {email} does not exists.");
            }

            user.Update(firstName, lastName, gender);

            await _userRepository.UpdateAsync(user);
        }

        private async Task<bool> EmailInUseAsync(string email)
        {
            var user = await _userRepository.GetAsync(email);
            return user != null;
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await _userRepository.GetAsync(id);
            if (user == null)
            {
                throw new ServiceException(ErrorCodes.UserNotFound, $"User with id: {id} does not exists.");
            }
            await _userRepository.DeleteAsync(id);
        }
    }
}