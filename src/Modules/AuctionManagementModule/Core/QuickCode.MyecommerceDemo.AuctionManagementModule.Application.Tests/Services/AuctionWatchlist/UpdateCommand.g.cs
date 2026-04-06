using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Application.Services.AuctionWatchlist;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Application.Dtos.AuctionWatchlist;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Application.Interfaces.Repositories;
using QuickCode.MyecommerceDemo.Common.Helpers;
using QuickCode.MyecommerceDemo.Common.Models;

namespace QuickCode.MyecommerceDemo.AuctionManagementModule.Application.Tests.Services.AuctionWatchlist
{
    public class UpdateAuctionWatchlistCommandTests : IDisposable
    {
        private const int ResultCodeSuccess = 0;
        private const int ResultCodeNotFound = 404;
        private readonly Mock<IAuctionWatchlistRepository> _repositoryMock;
        private readonly Mock<ILogger<AuctionWatchlistService>> _loggerMock;
        private readonly AuctionWatchlistService _service;
        public UpdateAuctionWatchlistCommandTests()
        {
            _repositoryMock = new Mock<IAuctionWatchlistRepository>();
            _loggerMock = new Mock<ILogger<AuctionWatchlistService>>();
            _service = new AuctionWatchlistService(_loggerMock.Object, _repositoryMock.Object);
        }

        [Fact]
        public async Task UpdateAsync_Should_Return_Success_When_Item_Exists()
        {
            // Arrange
            var fakeDto = TestDataGenerator.CreateFake<AuctionWatchlistDto>("tr");
            var fakeGetResponse = new RepoResponse<AuctionWatchlistDto>(fakeDto, "Success");
            var fakeUpdateResponse = new RepoResponse<bool>(true, "Success");
            _repositoryMock.Setup(r => r.GetByPkAsync(fakeDto.Id)).ReturnsAsync(fakeGetResponse);
            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<AuctionWatchlistDto>())).ReturnsAsync(fakeUpdateResponse);
            // Act
            var result = await _service.UpdateAsync(fakeDto.Id, fakeDto);
            // Assert
            Assert.Equal(ResultCodeSuccess, result.Code);
            Assert.True(result.Value);
            _repositoryMock.Verify(r => r.GetByPkAsync(fakeDto.Id), Times.Once);
            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<AuctionWatchlistDto>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_Should_Return_NotFound_When_Item_Does_Not_Exist()
        {
            // Arrange
            var fakeDto = TestDataGenerator.CreateFake<AuctionWatchlistDto>("tr");
            var fakeGetResponse = new RepoResponse<AuctionWatchlistDto>
            {
                Code = ResultCodeNotFound,
                Message = "Not found"
            };
            _repositoryMock.Setup(r => r.GetByPkAsync(fakeDto.Id)).ReturnsAsync(fakeGetResponse);
            // Act
            var result = await _service.UpdateAsync(fakeDto.Id, fakeDto);
            // Assert
            Assert.Equal(ResultCodeNotFound, result.Code);
            Assert.False(result.Value);
            _repositoryMock.Verify(r => r.GetByPkAsync(fakeDto.Id), Times.Once);
            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<AuctionWatchlistDto>()), Times.Never);
        }

        public void Dispose()
        {
        // Cleanup handled by xUnit
        }
    }
}