using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Application.Services.Bid;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Application.Dtos.Bid;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Application.Interfaces.Repositories;
using QuickCode.MyecommerceDemo.Common.Helpers;
using QuickCode.MyecommerceDemo.Common.Models;

namespace QuickCode.MyecommerceDemo.AuctionManagementModule.Application.Tests.Services.Bid
{
    public class BidServiceDeleteTests : IDisposable
    {
        private const int ResultCodeSuccess = 0;
        private const int ResultCodeNotFound = 404;
        private readonly Mock<IBidRepository> _repositoryMock;
        private readonly Mock<ILogger<BidService>> _loggerMock;
        private readonly BidService _service;
        public BidServiceDeleteTests()
        {
            _repositoryMock = new Mock<IBidRepository>();
            _loggerMock = new Mock<ILogger<BidService>>();
            _service = new BidService(_loggerMock.Object, _repositoryMock.Object);
        }

        [Fact]
        public async Task DeleteItemAsync_Should_Return_Success_When_Item_Exists()
        {
            // Arrange
            var fakeDto = TestDataGenerator.CreateFake<BidDto>("tr");
            var fakeGetResponse = new RepoResponse<BidDto>(fakeDto, "Success");
            var fakeDeleteResponse = new RepoResponse<bool>(true, "Success");
            _repositoryMock.Setup(r => r.GetByPkAsync(fakeDto.Id)).ReturnsAsync(fakeGetResponse);
            _repositoryMock.Setup(r => r.DeleteAsync(It.IsAny<BidDto>())).ReturnsAsync(fakeDeleteResponse);
            // Act
            var result = await _service.DeleteItemAsync(fakeDto.Id);
            // Assert
            Assert.Equal(ResultCodeSuccess, result.Code);
            Assert.True(result.Value);
            _repositoryMock.Verify(r => r.GetByPkAsync(fakeDto.Id), Times.Once);
            _repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<BidDto>()), Times.Once);
        }

        [Fact]
        public async Task DeleteItemAsync_Should_Return_NotFound_When_Item_Does_Not_Exist()
        {
            var fakeDto = TestDataGenerator.CreateFake<BidDto>("tr");
            // Arrange
            var fakeGetResponse = new RepoResponse<BidDto>
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
            _repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<BidDto>()), Times.Never);
        }

        public void Dispose()
        {
        // Cleanup handled by xUnit
        }
    }
}