using AutoMapper;
using CryptoInvestor.Core.Domain;
using CryptoInvestor.Core.Repositories;
using CryptoInvestor.Infrastructure.DTO;
using CryptoInvestor.Infrastructure.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoInvestor.Infrastructure.Services
{
    public class CoinService : ICoinService
    {
        private readonly ICoinRepository _coinRepository;
        private readonly IMapper _mapper;

        public CoinService(ICoinRepository coinRepository, IMapper mapper)
        {
            _coinRepository = coinRepository;
            _mapper = mapper;
        }

        public async Task<CoinDto> GetAsync(string symbol)
        {
            var coin = await _coinRepository.GetAsync(symbol);
            return _mapper.Map<CoinDto>(coin);
        }

        public async Task<IEnumerable<CoinDto>> BrowseAsync()
        {
            var coins = await _coinRepository.BrowseAsync();
            return _mapper.Map<IEnumerable<Coin>, IEnumerable<CoinDto>>(coins);
        }

        public async Task<IEnumerable<CoinShortDto>> BrowseShortAsync()
        {
            var coins = await _coinRepository.BrowseAsync();
            return _mapper.Map<IEnumerable<Coin>, IEnumerable<CoinShortDto>>(coins);
        }
    }
}