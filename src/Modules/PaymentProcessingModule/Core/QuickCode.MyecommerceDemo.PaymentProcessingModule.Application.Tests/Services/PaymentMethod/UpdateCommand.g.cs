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
    public class UpdatePaymentMethodCommandTests : IDisposable
    {
        private const int ResultCodeSuccess = 0;
        private const int ResultCodeNotFound = 404;
        private readonly Mock<IPaymentMethodRepository> _repositoryMock;
        private readonly Mock<ILogger<PaymentMethodService>> _loggerMock;
        private readonly PaymentMethodService _service;
        public UpdatePaymentMethodCommandTests()
        {
            _repositoryMock = new Mock<IPaymentMethodRepository>();
            _loggerMock = new Mock<ILogger<PaymentMethodService>>();
            _service = new PaymentMethodService(_loggerMock.Object, _repositoryMock.Object);
        }

        [Fact]
        public async Task UpdateAsync_Should_Return_Success_When_Item_Exists()
        {
            // Arrange
            var fakeDto = TestDataGenerator.CreateFake<PaymentMethodDto>("tr");
            var fakeGetResponse = new RepoResponse<PaymentMethodDto>(fakeDto, "Success");
            var fakeUpdateResponse = new RepoResponse<bool>(true, "Success");
            _repositoryMock.Setup(r => r.GetByPkAsync(fakeDto.Id)).ReturnsAsync(fakeGetResponse);
            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<PaymentMethodDto>())).ReturnsAsync(fakeUpdateResponse);
            // Act
            var result = await _service.UpdateAsync(fakeDto.Id, fakeDto);
            // Assert
            Assert.Equal(ResultCodeSuccess, result.Code);
            Assert.True(result.Value);
            _repositoryMock.Verify(r => r.GetByPkAsync(fakeDto.Id), Times.Once);
            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<PaymentMethodDto>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_Should_Return_NotFound_When_Item_Does_Not_Exist()
        {
            // Arrange
            var fakeDto = TestDataGenerator.CreateFake<PaymentMethodDto>("tr");
            var fakeGetResponse = new RepoResponse<PaymentMethodDto>
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
            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<PaymentMethodDto>()), Times.Never);
        }

        public void Dispose()
        {
        // Cleanup handled by xUnit
        }
    }
}