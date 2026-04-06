using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using QuickCode.MyecommerceDemo.ProductCatalogModule.Application.Services.Product;
using QuickCode.MyecommerceDemo.ProductCatalogModule.Application.Dtos.Product;
using QuickCode.MyecommerceDemo.ProductCatalogModule.Application.Interfaces.Repositories;
using QuickCode.MyecommerceDemo.Common.Helpers;
using QuickCode.MyecommerceDemo.Common.Models;

namespace QuickCode.MyecommerceDemo.ProductCatalogModule.Application.Tests.Services.Product
{
    public class InsertProductCommandTests : IDisposable
    {
        private const int ResultCodeSuccess = 0;
        private const int ResultCodeNotFound = 404;
        private readonly Mock<IProductRepository> _repositoryMock;
        private readonly Mock<ILogger<ProductService>> _loggerMock;
        private readonly ProductService _service;
        public InsertProductCommandTests()
        {
            _repositoryMock = new Mock<IProductRepository>();
            _loggerMock = new Mock<ILogger<ProductService>>();
            _service = new ProductService(_loggerMock.Object, _repositoryMock.Object);
        }

        [Fact]
        public async Task InsertAsync_Should_Return_Success_When_Valid_Request()
        {
            // Arrange
            var fakeDto = TestDataGenerator.CreateFake<ProductDto>("tr");
            var fakeResponse = new RepoResponse<ProductDto>(fakeDto, "Success");
            _repositoryMock.Setup(r => r.InsertAsync(It.IsAny<ProductDto>())).ReturnsAsync(fakeResponse);
            // Act
            var result = await _service.InsertAsync(fakeDto);
            // Assert
            Assert.Equal(ResultCodeSuccess, result.Code);
            Assert.Equal(fakeDto, result.Value);
            _repositoryMock.Verify(r => r.InsertAsync(It.IsAny<ProductDto>()), Times.Once);
        }

        [Fact]
        public async Task InsertAsync_Should_Return_NotFound_When_Repository_Returns_404()
        {
            // Arrange
            var fakeDto = TestDataGenerator.CreateFake<ProductDto>("tr");
            var fakeResponse = new RepoResponse<ProductDto>
            {
                Code = ResultCodeNotFound,
                Message = "Not found"
            };
            _repositoryMock.Setup(r => r.InsertAsync(It.IsAny<ProductDto>())).ReturnsAsync(fakeResponse);
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