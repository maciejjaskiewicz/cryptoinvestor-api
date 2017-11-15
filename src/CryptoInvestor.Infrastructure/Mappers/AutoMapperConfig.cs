using AutoMapper;
using CryptoInvestor.Core.Domain;
using CryptoInvestor.Infrastructure.DTO;

namespace CryptoInvestor.Infrastructure.Mappers
{
    public static class AutoMapperConfig
    {
        public static IMapper Initialize()
        {
            var mapperCfg = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDto>();
                cfg.CreateMap<User, UserDetailsDto>();
                cfg.CreateMap<Portfolio, PortfolioDto>();
                cfg.CreateMap<Coin, CoinDto>();
                cfg.CreateMap<Coin, CoinShortDto>();
                cfg.CreateMap<CoinPrice, CoinPriceDto>();
                cfg.CreateMap<Favourites, FavouritesDto>();
                cfg.CreateMap<Transaction, TransactionDto>();
            });

            return mapperCfg.CreateMapper();
        }
    }
}