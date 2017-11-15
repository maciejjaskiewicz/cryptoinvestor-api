using AutoMapper;
using CryptoInvestor.Core.Domain;
using CryptoInvestor.Core.Repositories;
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
    public class PortfolioServiceTest
    {
        private readonly Mock<IPortfolioRepository> _portfolioRepository;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IMapper> _mapper;

        public PortfolioServiceTest()
        {
            _portfolioRepository = new Mock<IPortfolioRepository>();
            _userRepository = new Mock<IUserRepository>();
            _mapper = new Mock<IMapper>();
        }

        [Fact]
        public async Task GetAsync_should_invoke_GetAsync_on_repository()
        {
            var portfolio = new Portfolio(Guid.NewGuid(), "default");
            _portfolioRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(portfolio);
            var portfolioService = new PortfolioService(_portfolioRepository.Object, _userRepository.Object, _mapper.Object);

            await portfolioService.GetAsync(Guid.NewGuid());

            _portfolioRepository.Verify(x => x.GetAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task GetAsync_should_return_PortfolioDto()
        {
            var portfolio = new Portfolio(Guid.NewGuid(), "default");
            _portfolioRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(portfolio);
            _mapper.Setup(x => x.Map<PortfolioDto>(It.IsAny<Portfolio>())).Returns(new PortfolioDto());
            var portfolioService = new PortfolioService(_portfolioRepository.Object, _userRepository.Object, _mapper.Object);

            var portfolioDto = await portfolioService.GetAsync(Guid.NewGuid());

            portfolioDto.ShouldBeOfType<PortfolioDto>();
        }

        [Fact]
        public async Task GetAsync_by_name_should_invoke_BrowseAsync_on_repository()
        {
            var portfolio = new Portfolio(Guid.NewGuid(), "default");
            _portfolioRepository.Setup(x => x.BrowseAsync(It.IsAny<Guid>())).ReturnsAsync(new[] { portfolio });
            var portfolioService = new PortfolioService(_portfolioRepository.Object, _userRepository.Object, _mapper.Object);

            await portfolioService.GetAsync(Guid.NewGuid(), "default");

            _portfolioRepository.Verify(x => x.BrowseAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task GetAsync_by_name_should_return_PortfolioDto()
        {
            var portfolio = new Portfolio(Guid.NewGuid(), "default");
            _portfolioRepository.Setup(x => x.BrowseAsync(It.IsAny<Guid>())).ReturnsAsync(new[] { portfolio });
            _mapper.Setup(x => x.Map<PortfolioDto>(It.IsAny<Portfolio>())).Returns(new PortfolioDto());
            var portfolioService = new PortfolioService(_portfolioRepository.Object, _userRepository.Object, _mapper.Object);

            var portfolioDto = await portfolioService.GetAsync(Guid.NewGuid(), "default");

            portfolioDto.ShouldBeOfType<PortfolioDto>();
        }

        [Fact]
        public async Task BrowseAsync_should_invoke_BrowseAsync_on_repository()
        {
            var portfolio = new Portfolio(Guid.NewGuid(), "default");
            _portfolioRepository.Setup(x => x.BrowseAsync(It.IsAny<Guid>())).ReturnsAsync(new[] { portfolio });
            var portfolioService = new PortfolioService(_portfolioRepository.Object, _userRepository.Object, _mapper.Object);

            await portfolioService.BrowseAsync(Guid.NewGuid());

            _portfolioRepository.Verify(x => x.BrowseAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task BrowseAsync_should_return_IEnumerable_collection_of_PortfolioDto()
        {
            var portfolio = new Portfolio(Guid.NewGuid(), "default");
            _portfolioRepository.Setup(x => x.BrowseAsync(It.IsAny<Guid>())).ReturnsAsync(new[] { portfolio });
            _mapper.Setup(x => x.Map<IEnumerable<PortfolioDto>>(It.IsAny<IEnumerable<Portfolio>>())).Returns(new[] { new PortfolioDto() });
            var portfolioService = new PortfolioService(_portfolioRepository.Object, _userRepository.Object, _mapper.Object);

            var portfolioDtos = await portfolioService.BrowseAsync(Guid.NewGuid());

            portfolioDtos.ShouldBeAssignableTo<IEnumerable<PortfolioDto>>();
        }

        [Fact]
        public async Task CreateAsync_should_invoke_AddAsync_on_repository()
        {
            var user = new User("test@email.com", "test", "secret", "salt");
            var portfolio = new Portfolio(Guid.NewGuid(), "default");
            _userRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(user);
            _portfolioRepository.Setup(x => x.BrowseAsync(It.IsAny<Guid>())).ReturnsAsync(new[] { portfolio });
            var portfolioService = new PortfolioService(_portfolioRepository.Object, _userRepository.Object, _mapper.Object);

            await portfolioService.CreateAsync(Guid.NewGuid(), "test");

            _portfolioRepository.Verify(x => x.AddAsync(It.IsAny<Portfolio>()), Times.Once);
        }

        [Fact]
        public async Task Given_not_exists_user_CreateAsync_should_throw_exception()
        {
            var portfolio = new Portfolio(Guid.NewGuid(), "default");
            _userRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync((User)null);
            _portfolioRepository.Setup(x => x.BrowseAsync(It.IsAny<Guid>())).ReturnsAsync(new[] { portfolio });
            var portfolioService = new PortfolioService(_portfolioRepository.Object, _userRepository.Object, _mapper.Object);

            ServiceException ex = await Should.ThrowAsync<ServiceException>(async () =>
            {
                await portfolioService.CreateAsync(Guid.NewGuid(), "test");
            });

            ex.Code.ShouldBe(ErrorCodes.UserNotFound);
        }

        [Fact]
        public async Task Given_portfolio_with_name_in_use_CreateAsync_should_throw_exception()
        {
            var user = new User("test@email.com", "test", "secret", "salt");
            var portfolio = new Portfolio(Guid.NewGuid(), "default");
            _userRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(user);
            _portfolioRepository.Setup(x => x.BrowseAsync(It.IsAny<Guid>())).ReturnsAsync(new[] { portfolio });
            var portfolioService = new PortfolioService(_portfolioRepository.Object, _userRepository.Object, _mapper.Object);

            ServiceException ex = await Should.ThrowAsync<ServiceException>(async () =>
            {
                await portfolioService.CreateAsync(Guid.NewGuid(), "default");
            });

            ex.Code.ShouldBe(ErrorCodes.PortfolioNameInUse);
        }

        [Fact]
        public async Task UpdateAsync_should_invoke_UpdateAsync_on_repository()
        {
            var portfolio = new Portfolio(Guid.NewGuid(), "default");
            _portfolioRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(portfolio);
            var portfolioService = new PortfolioService(_portfolioRepository.Object, _userRepository.Object, _mapper.Object);

            await portfolioService.UpdateAsync(Guid.NewGuid(), "default");

            _portfolioRepository.Verify(x => x.UpdateAsync(It.IsAny<Portfolio>()), Times.Once);
        }

        [Fact]
        public async Task Given_not_exists_portfolio_UpdateAsync_should_throw_exception()
        {
            _portfolioRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync((Portfolio)null);
            var portfolioService = new PortfolioService(_portfolioRepository.Object, _userRepository.Object, _mapper.Object);

            ServiceException ex = await Should.ThrowAsync<ServiceException>(async () =>
            {
                await portfolioService.UpdateAsync(Guid.NewGuid(), "default");
            });

            ex.Code.ShouldBe(ErrorCodes.PortfolioNotFound);
        }

        [Fact]
        public async Task DeleteAsync_should_invoke_DeleteAsync_on_repository()
        {
            var portfolio = new Portfolio(Guid.NewGuid(), "default");
            _portfolioRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(portfolio);
            var portfolioService = new PortfolioService(_portfolioRepository.Object, _userRepository.Object, _mapper.Object);

            await portfolioService.DeleteAsync(Guid.NewGuid());

            _portfolioRepository.Verify(x => x.DeleteAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task Given_not_exists_portfolio_DeleteAsync_should_throw_exception()
        {
            _portfolioRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync((Portfolio)null);
            var portfolioService = new PortfolioService(_portfolioRepository.Object, _userRepository.Object, _mapper.Object);

            ServiceException ex = await Should.ThrowAsync<ServiceException>(async () =>
            {
                await portfolioService.DeleteAsync(Guid.NewGuid());
            });

            ex.Code.ShouldBe(ErrorCodes.PortfolioNotFound);
        }
    }
}