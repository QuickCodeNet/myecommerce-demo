using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using QuickCode.MyecommerceDemo.OrderManagementModule.Application.Services.OrderNote;
using QuickCode.MyecommerceDemo.OrderManagementModule.Application.Dtos.OrderNote;
using QuickCode.MyecommerceDemo.OrderManagementModule.Application.Interfaces.Repositories;
using QuickCode.MyecommerceDemo.Common.Helpers;
using QuickCode.MyecommerceDemo.Common.Models;

namespace QuickCode.MyecommerceDemo.OrderManagementModule.Application.Tests.Services.OrderNote
{
    public class InsertOrderNoteCommandTests : IDisposable
    {
        private const int ResultCodeSuccess = 0;
        private const int ResultCodeNotFound = 404;
        private readonly Mock<IOrderNoteRepository> _repositoryMock;
        private readonly Mock<ILogger<OrderNoteService>> _loggerMock;
        private readonly OrderNoteService _service;
        public InsertOrderNoteCommandTests()
        {
            _repositoryMock = new Mock<IOrderNoteRepository>();
            _loggerMock = new Mock<ILogger<OrderNoteService>>();
            _service = new OrderNoteService(_loggerMock.Object, _repositoryMock.Object);
        }

        [Fact]
        public async Task InsertAsync_Should_Return_Success_When_Valid_Request()
        {
            // Arrange
            var fakeDto = TestDataGenerator.CreateFake<OrderNoteDto>("tr");
            var fakeResponse = new RepoResponse<OrderNoteDto>(fakeDto, "Success");
            _repositoryMock.Setup(r => r.InsertAsync(It.IsAny<OrderNoteDto>())).ReturnsAsync(fakeResponse);
            // Act
            var result = await _service.InsertAsync(fakeDto);
            // Assert
            Assert.Equal(ResultCodeSuccess, result.Code);
            Assert.Equal(fakeDto, result.Value);
            _repositoryMock.Verify(r => r.InsertAsync(It.IsAny<OrderNoteDto>()), Times.Once);
        }

        [Fact]
        public async Task InsertAsync_Should_Return_NotFound_When_Repository_Returns_404()
        {
            // Arrange
            var fakeDto = TestDataGenerator.CreateFake<OrderNoteDto>("tr");
            var fakeResponse = new RepoResponse<OrderNoteDto>
            {
                Code = ResultCodeNotFound,
                Message = "Not found"
            };
            _repositoryMock.Setup(r => r.InsertAsync(It.IsAny<OrderNoteDto>())).ReturnsAsync(fakeResponse);
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