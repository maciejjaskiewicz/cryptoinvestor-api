using AutoMapper;
using CryptoInvestor.Core.Domain;
using CryptoInvestor.Core.Repositories;
using CryptoInvestor.Infrastructure.DTO;
using CryptoInvestor.Infrastructure.Exceptions;
using CryptoInvestor.Infrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoInvestor.Infrastructure.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public PortfolioService(IPortfolioRepository portfolioRepository,
            IUserRepository userRepository, IMapper mapper)
        {
            _portfolioRepository = portfolioRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<PortfolioDto> GetAsync(Guid id)
        {
            var portfolio = await _portfolioRepository.GetAsync(id);

            return _mapper.Map<PortfolioDto>(portfolio);
        }

        public async Task<PortfolioDto> GetAsync(Guid userId, string nameId)
        {
            var portfolios = await _portfolioRepository.BrowseAsync(userId);
            var portfolio = portfolios.SingleOrDefault(x => x.NameId == nameId);

            return _mapper.Map<PortfolioDto>(portfolio);
        }

        public async Task<IEnumerable<PortfolioDto>> BrowseAsync(Guid userId)
        {
            var portfolios = await _portfolioRepository.BrowseAsync(userId);

            return _mapper.Map<IEnumerable<PortfolioDto>>(portfolios);
        }

        public async Task CreateAsync(Guid userId, string name)
        {
            var user = await _userRepository.GetAsync(userId);
            if (user == null)
            {
                throw new ServiceException(ErrorCodes.UserNotFound,
                    $"User with id: {userId} does not exists.");
            }

            var portfolio = new Portfolio(userId, name);
            var inUse = await NameIdInUseAsync(userId, portfolio.NameId);
            if (inUse)
            {
                throw new ServiceException(ErrorCodes.PortfolioNameInUse,
                    $"Portfolio with name: {name} already exists.");
            }

            await _portfolioRepository.AddAsync(portfolio);
        }

        public async Task UpdateAsync(Guid id, string name)
        {
            var portfolio = await _portfolioRepository.GetAsync(id);
            if (portfolio == null)
            {
                throw new ServiceException(ErrorCodes.PortfolioNotFound,
                    $"Portfolio with id: {id} does not exists.");
            }

            portfolio.SetName(name);

            await _portfolioRepository.UpdateAsync(portfolio);
        }

        public async Task DeleteAsync(Guid id)
        {
            var portfolio = await _portfolioRepository.GetAsync(id);
            if (portfolio == null)
            {
                throw new ServiceException(ErrorCodes.PortfolioNotFound,
                    $"Portfolio with id: {id} does not exists.");
            }

            await _portfolioRepository.DeleteAsync(id);
        }

        private async Task<bool> NameIdInUseAsync(Guid userId, string nameId)
        {
            var portfolios = await _portfolioRepository.BrowseAsync(userId);

            return portfolios.Any(x => x.NameId == nameId);
        }
    }
}