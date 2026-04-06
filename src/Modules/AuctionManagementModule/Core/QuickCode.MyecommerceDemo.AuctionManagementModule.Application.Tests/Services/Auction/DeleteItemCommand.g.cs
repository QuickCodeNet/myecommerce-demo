using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Application.Services.Auction;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Application.Dtos.Auction;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Application.Interfaces.Repositories;
using QuickCode.MyecommerceDemo.Common.Helpers;
using QuickCode.MyecommerceDemo.Common.Models;

namespace QuickCode.MyecommerceDemo.AuctionManagementModule.Application.Tests.Services.Auction
{
    public class AuctionServiceDeleteTests : IDisposable
    {
        private const int ResultCodeSuccess = 0;
        private const int ResultCodeNotFound = 404;
        private readonly Mock<IAuctionRepository> _repositoryMock;
        private readonly Mock<ILogger<AuctionService>> _loggerMock;
        private readonly AuctionService _service;
        public AuctionServiceDeleteTests()
        {
            _repositoryMock = new Mock<IAuctionRepository>();
            _loggerMock = new Mock<ILogger<AuctionService>>();
            _service = new AuctionService(_loggerMock.Object, _repositoryMock.Object);
        }

        [Fact]
        public async Task DeleteItemAsync_Should_Return_Success_When_Item_Exists()
        {
            // Arrange
            var fakeDto = TestDataGenerator.CreateFake<AuctionDto>("tr");
            var fakeGetResponse = new RepoResponse<AuctionDto>(fakeDto, "Success");
            var fakeDeleteResponse = new RepoResponse<bool>(true, "Success");
            _repositoryMock.Setup(r => r.GetByPkAsync(fakeDto.Id)).ReturnsAsync(fakeGetResponse);
            _repositoryMock.Setup(r => r.DeleteAsync(It.IsAny<AuctionDto>())).ReturnsAsync(fakeDeleteResponse);
            // Act
            var result = await _service.DeleteItemAsync(fakeDto.Id);
            // Assert
            Assert.Equal(ResultCodeSuccess, result.Code);
            Assert.True(result.Value);
            _repositoryMock.Verify(r => r.GetByPkAsync(fakeDto.Id), Times.Once);
            _repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<AuctionDto>()), Times.Once);
        }

        [Fact]
        public async Task DeleteItemAsync_Should_Return_NotFound_When_Item_Does_Not_Exist()
        {
            var fakeDto = TestDataGenerator.CreateFake<AuctionDto>("tr");
            // Arrange
            var fakeGetResponse = new RepoResponse<AuctionDto>
            {
                Code = ResultCodeNotFound,
                Message = "Not found"
            };
            _repositoryMock.Setup(r => r.GetByPkAsync(fakeDto.Id)).ReturnsAsync(fakeGetResponse);
            // Act
            var result = await _service.DeleteItemAsync(fakeDto.Id);
            // Assert
            Assert.Equal(ResultCodeNotFound, result.Code);
            Assert.False(result.Value);
            _repositoryMock.Verify(r => r.GetByPkAsync(fakeDto.Id), Times.Once);
            _repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<AuctionDto>()), Times.Never);
        }

        public void Dispose()
        {
        // Cleanup handled by xUnit
        }
    }
}