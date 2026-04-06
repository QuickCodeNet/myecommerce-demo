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
    public class OrderStatusHistoryServiceDeleteTests : IDisposable
    {
        private const int ResultCodeSuccess = 0;
        private const int ResultCodeNotFound = 404;
        private readonly Mock<IOrderStatusHistoryRepository> _repositoryMock;
        private readonly Mock<ILogger<OrderStatusHistoryService>> _loggerMock;
        private readonly OrderStatusHistoryService _service;
        public OrderStatusHistoryServiceDeleteTests()
        {
            _repositoryMock = new Mock<IOrderStatusHistoryRepository>();
            _loggerMock = new Mock<ILogger<OrderStatusHistoryService>>();
            _service = new OrderStatusHistoryService(_loggerMock.Object, _repositoryMock.Object);
        }

        [Fact]
        public async Task DeleteItemAsync_Should_Return_Success_When_Item_Exists()
        {
            // Arrange
            var fakeDto = TestDataGenerator.CreateFake<OrderStatusHistoryDto>("tr");
            var fakeGetResponse = new RepoResponse<OrderStatusHistoryDto>(fakeDto, "Success");
            var fakeDeleteResponse = new RepoResponse<bool>(true, "Success");
            _repositoryMock.Setup(r => r.GetByPkAsync(fakeDto.Id)).ReturnsAsync(fakeGetResponse);
            _repositoryMock.Setup(r => r.DeleteAsync(It.IsAny<OrderStatusHistoryDto>())).ReturnsAsync(fakeDeleteResponse);
            // Act
            var result = await _service.DeleteItemAsync(fakeDto.Id);
            // Assert
            Assert.Equal(ResultCodeSuccess, result.Code);
            Assert.True(result.Value);
            _repositoryMock.Verify(r => r.GetByPkAsync(fakeDto.Id), Times.Once);
            _repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<OrderStatusHistoryDto>()), Times.Once);
        }

        [Fact]
        public async Task DeleteItemAsync_Should_Return_NotFound_When_Item_Does_Not_Exist()
        {
            var fakeDto = TestDataGenerator.CreateFake<OrderStatusHistoryDto>("tr");
            // Arrange
            var fakeGetResponse = new RepoResponse<OrderStatusHistoryDto>
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
            _repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<OrderStatusHistoryDto>()), Times.Never);
        }

        public void Dispose()
        {
        // Cleanup handled by xUnit
        }
    }
}