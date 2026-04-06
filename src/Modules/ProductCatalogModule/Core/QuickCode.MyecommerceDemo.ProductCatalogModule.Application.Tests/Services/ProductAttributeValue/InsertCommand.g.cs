using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using QuickCode.MyecommerceDemo.ProductCatalogModule.Application.Services.ProductAttributeValue;
using QuickCode.MyecommerceDemo.ProductCatalogModule.Application.Dtos.ProductAttributeValue;
using QuickCode.MyecommerceDemo.ProductCatalogModule.Application.Interfaces.Repositories;
using QuickCode.MyecommerceDemo.Common.Helpers;
using QuickCode.MyecommerceDemo.Common.Models;

namespace QuickCode.MyecommerceDemo.ProductCatalogModule.Application.Tests.Services.ProductAttributeValue
{
    public class InsertProductAttributeValueCommandTests : IDisposable
    {
        private const int ResultCodeSuccess = 0;
        private const int ResultCodeNotFound = 404;
        private readonly Mock<IProductAttributeValueRepository> _repositoryMock;
        private readonly Mock<ILogger<ProductAttributeValueService>> _loggerMock;
        private readonly ProductAttributeValueService _service;
        public InsertProductAttributeValueCommandTests()
        {
            _repositoryMock = new Mock<IProductAttributeValueRepository>();
            _loggerMock = new Mock<ILogger<ProductAttributeValueService>>();
            _service = new ProductAttributeValueService(_loggerMock.Object, _repositoryMock.Object);
        }

        [Fact]
        public async Task InsertAsync_Should_Return_Success_When_Valid_Request()
        {
            // Arrange
            var fakeDto = TestDataGenerator.CreateFake<ProductAttributeValueDto>("tr");
            var fakeResponse = new RepoResponse<ProductAttributeValueDto>(fakeDto, "Success");
            _repositoryMock.Setup(r => r.InsertAsync(It.IsAny<ProductAttributeValueDto>())).ReturnsAsync(fakeResponse);
            // Act
            var result = await _service.InsertAsync(fakeDto);
            // Assert
            Assert.Equal(ResultCodeSuccess, result.Code);
            Assert.Equal(fakeDto, result.Value);
            _repositoryMock.Verify(r => r.InsertAsync(It.IsAny<ProductAttributeValueDto>()), Times.Once);
        }

        [Fact]
        public async Task InsertAsync_Should_Return_NotFound_When_Repository_Returns_404()
        {
            // Arrange
            var fakeDto = TestDataGenerator.CreateFake<ProductAttributeValueDto>("tr");
            var fakeResponse = new RepoResponse<ProductAttributeValueDto>
            {
                Code = ResultCodeNotFound,
                Message = "Not found"
            };
            _repositoryMock.Setup(r => r.InsertAsync(It.IsAny<ProductAttributeValueDto>())).ReturnsAsync(fakeResponse);
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