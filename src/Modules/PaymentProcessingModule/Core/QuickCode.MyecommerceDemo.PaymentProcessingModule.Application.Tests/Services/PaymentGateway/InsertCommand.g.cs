using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using QuickCode.MyecommerceDemo.PaymentProcessingModule.Application.Services.PaymentGateway;
using QuickCode.MyecommerceDemo.PaymentProcessingModule.Application.Dtos.PaymentGateway;
using QuickCode.MyecommerceDemo.PaymentProcessingModule.Application.Interfaces.Repositories;
using QuickCode.MyecommerceDemo.Common.Helpers;
using QuickCode.MyecommerceDemo.Common.Models;

namespace QuickCode.MyecommerceDemo.PaymentProcessingModule.Application.Tests.Services.PaymentGateway
{
    public class InsertPaymentGatewayCommandTests : IDisposable
    {
        private const int ResultCodeSuccess = 0;
        private const int ResultCodeNotFound = 404;
        private readonly Mock<IPaymentGatewayRepository> _repositoryMock;
        private readonly Mock<ILogger<PaymentGatewayService>> _loggerMock;
        private readonly PaymentGatewayService _service;
        public InsertPaymentGatewayCommandTests()
        {
            _repositoryMock = new Mock<IPaymentGatewayRepository>();
            _loggerMock = new Mock<ILogger<PaymentGatewayService>>();
            _service = new PaymentGatewayService(_loggerMock.Object, _repositoryMock.Object);
        }

        [Fact]
        public async Task InsertAsync_Should_Return_Success_When_Valid_Request()
        {
            // Arrange
            var fakeDto = TestDataGenerator.CreateFake<PaymentGatewayDto>("tr");
            var fakeResponse = new RepoResponse<PaymentGatewayDto>(fakeDto, "Success");
            _repositoryMock.Setup(r => r.InsertAsync(It.IsAny<PaymentGatewayDto>())).ReturnsAsync(fakeResponse);
            // Act
            var result = await _service.InsertAsync(fakeDto);
            // Assert
            Assert.Equal(ResultCodeSuccess, result.Code);
            Assert.Equal(fakeDto, result.Value);
            _repositoryMock.Verify(r => r.InsertAsync(It.IsAny<PaymentGatewayDto>()), Times.Once);
        }

        [Fact]
        public async Task InsertAsync_Should_Return_NotFound_When_Repository_Returns_404()
        {
            // Arrange
            var fakeDto = TestDataGenerator.CreateFake<PaymentGatewayDto>("tr");
            var fakeResponse = new RepoResponse<PaymentGatewayDto>
            {
                Code = ResultCodeNotFound,
                Message = "Not found"
            };
            _repositoryMock.Setup(r => r.InsertAsync(It.IsAny<PaymentGatewayDto>())).ReturnsAsync(fakeResponse);
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