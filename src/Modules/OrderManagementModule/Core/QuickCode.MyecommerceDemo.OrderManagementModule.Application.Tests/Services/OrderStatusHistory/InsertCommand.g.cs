using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using QuickCode.MyecommerceDemo.OrderManagementModule.Application.Services.OrderStatusHistory;
using QuickCode.MyecommerceDemo.OrderManagementModule.Application.Dtos.OrderStatusHistory;
using QuickCode.MyecommerceDemo.OrderManagementModule.Application.Interfaces.Repositories;
using QuickCode.MyecommerceDemo.Common.Helpers;
using QuickCode.MyecommerceDemo.Common.Models;

namespace QuickCode.MyecommerceDemo.OrderManagementModule.Application.Tests.Services.OrderStatusHistory
{
    public class InsertOrderStatusHistoryCommandTests : IDisposable
    {
        private const int ResultCodeSuccess = 0;
        private const int ResultCodeNotFound = 404;
        private readonly Mock<IOrderStatusHistoryRepository> _repositoryMock;
        private readonly Mock<ILogger<OrderStatusHistoryService>> _loggerMock;
        private readonly OrderStatusHistoryService _service;
        public InsertOrderStatusHistoryCommandTests()
        {
            _repositoryMock = new Mock<IOrderStatusHistoryRepository>();
            _loggerMock = new Mock<ILogger<OrderStatusHistoryService>>();
            _service = new OrderStatusHistoryService(_loggerMock.Object, _repositoryMock.Object);
        }

        [Fact]
        public async Task InsertAsync_Should_Return_Success_When_Valid_Request()
        {
            // Arrange
            var fakeDto = TestDataGenerator.CreateFake<OrderStatusHistoryDto>("tr");
            var fakeResponse = new RepoResponse<OrderStatusHistoryDto>(fakeDto, "Success");
            _repositoryMock.Setup(r => r.InsertAsync(It.IsAny<OrderStatusHistoryDto>())).ReturnsAsync(fakeResponse);
            // Act
            var result = await _service.InsertAsync(fakeDto);
            // Assert
            Assert.Equal(ResultCodeSuccess, result.Code);
            Assert.Equal(fakeDto, result.Value);
            _repositoryMock.Verify(r => r.InsertAsync(It.IsAny<OrderStatusHistoryDto>()), Times.Once);
        }

        [Fact]
        public async Task InsertAsync_Should_Return_NotFound_When_Repository_Returns_404()
        {
            // Arrange
            var fakeDto = TestDataGenerator.CreateFake<OrderStatusHistoryDto>("tr");
            var fakeResponse = new RepoResponse<OrderStatusHistoryDto>
            {
                Code = ResultCodeNotFound,
                Message = "Not found"
            };
            _repositoryMock.Setup(r => r.InsertAsync(It.IsAny<OrderStatusHistoryDto>())).ReturnsAsync(fakeResponse);
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