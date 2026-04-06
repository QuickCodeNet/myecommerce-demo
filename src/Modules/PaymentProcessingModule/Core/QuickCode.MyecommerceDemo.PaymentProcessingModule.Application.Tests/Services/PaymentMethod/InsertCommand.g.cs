using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using QuickCode.MyecommerceDemo.PaymentProcessingModule.Application.Services.PaymentMethod;
using QuickCode.MyecommerceDemo.PaymentProcessingModule.Application.Dtos.PaymentMethod;
using QuickCode.MyecommerceDemo.PaymentProcessingModule.Application.Interfaces.Repositories;
using QuickCode.MyecommerceDemo.Common.Helpers;
using QuickCode.MyecommerceDemo.Common.Models;

namespace QuickCode.MyecommerceDemo.PaymentProcessingModule.Application.Tests.Services.PaymentMethod
{
    public class InsertPaymentMethodCommandTests : IDisposable
    {
        private const int ResultCodeSuccess = 0;
        private const int ResultCodeNotFound = 404;
        private readonly Mock<IPaymentMethodRepository> _repositoryMock;
        private readonly Mock<ILogger<PaymentMethodService>> _loggerMock;
        private readonly PaymentMethodService _service;
        public InsertPaymentMethodCommandTests()
        {
            _repositoryMock = new Mock<IPaymentMethodRepository>();
            _loggerMock = new Mock<ILogger<PaymentMethodService>>();
            _service = new PaymentMethodService(_loggerMock.Object, _repositoryMock.Object);
        }

        [Fact]
        public async Task InsertAsync_Should_Return_Success_When_Valid_Request()
        {
            // Arrange
            var fakeDto = TestDataGenerator.CreateFake<PaymentMethodDto>("tr");
            var fakeResponse = new RepoResponse<PaymentMethodDto>(fakeDto, "Success");
            _repositoryMock.Setup(r => r.InsertAsync(It.IsAny<PaymentMethodDto>())).ReturnsAsync(fakeResponse);
            // Act
            var result = await _service.InsertAsync(fakeDto);
            // Assert
            Assert.Equal(ResultCodeSuccess, result.Code);
            Assert.Equal(fakeDto, result.Value);
            _repositoryMock.Verify(r => r.InsertAsync(It.IsAny<PaymentMethodDto>()), Times.Once);
        }

        [Fact]
        public async Task InsertAsync_Should_Return_NotFound_When_Repository_Returns_404()
        {
            // Arrange
            var fakeDto = TestDataGenerator.CreateFake<PaymentMethodDto>("tr");
            var fakeResponse = new RepoResponse<PaymentMethodDto>
            {
                Code = ResultCodeNotFound,
                Message = "Not found"
            };
            _repositoryMock.Setup(r => r.InsertAsync(It.IsAny<PaymentMethodDto>())).ReturnsAsync(fakeResponse);
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