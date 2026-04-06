using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Application.Services.BidIncrementRule;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Application.Dtos.BidIncrementRule;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Application.Interfaces.Repositories;
using QuickCode.MyecommerceDemo.Common.Helpers;
using QuickCode.MyecommerceDemo.Common.Models;

namespace QuickCode.MyecommerceDemo.AuctionManagementModule.Application.Tests.Services.BidIncrementRule
{
    public class InsertBidIncrementRuleCommandTests : IDisposable
    {
        private const int ResultCodeSuccess = 0;
        private const int ResultCodeNotFound = 404;
        private readonly Mock<IBidIncrementRuleRepository> _repositoryMock;
        private readonly Mock<ILogger<BidIncrementRuleService>> _loggerMock;
        private readonly BidIncrementRuleService _service;
        public InsertBidIncrementRuleCommandTests()
        {
            _repositoryMock = new Mock<IBidIncrementRuleRepository>();
            _loggerMock = new Mock<ILogger<BidIncrementRuleService>>();
            _service = new BidIncrementRuleService(_loggerMock.Object, _repositoryMock.Object);
        }

        [Fact]
        public async Task InsertAsync_Should_Return_Success_When_Valid_Request()
        {
            // Arrange
            var fakeDto = TestDataGenerator.CreateFake<BidIncrementRuleDto>("tr");
            var fakeResponse = new RepoResponse<BidIncrementRuleDto>(fakeDto, "Success");
            _repositoryMock.Setup(r => r.InsertAsync(It.IsAny<BidIncrementRuleDto>())).ReturnsAsync(fakeResponse);
            // Act
            var result = await _service.InsertAsync(fakeDto);
            // Assert
            Assert.Equal(ResultCodeSuccess, result.Code);
            Assert.Equal(fakeDto, result.Value);
            _repositoryMock.Verify(r => r.InsertAsync(It.IsAny<BidIncrementRuleDto>()), Times.Once);
        }

        [Fact]
        public async Task InsertAsync_Should_Return_NotFound_When_Repository_Returns_404()
        {
            // Arrange
            var fakeDto = TestDataGenerator.CreateFake<BidIncrementRuleDto>("tr");
            var fakeResponse = new RepoResponse<BidIncrementRuleDto>
            {
                Code = ResultCodeNotFound,
                Message = "Not found"
            };
            _repositoryMock.Setup(r => r.InsertAsync(It.IsAny<BidIncrementRuleDto>())).ReturnsAsync(fakeResponse);
            // Act
            var result = await _service.InsertAsync(fakeDto);
            // Assert
            Assert.Equal(ResultCodeNotFound, result.Code);
            Assert.Null(result.Value);
        }

        public void Dispose()
        {
        // Cleanup handled by xUnit
        }
    }
}