using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Features.AspNetUserLogin;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos.AspNetUserLogin;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Interfaces.Repositories;
using QuickCode.MyecommerceDemo.Common.Helpers;
using QuickCode.MyecommerceDemo.Common.Models;

namespace QuickCode.MyecommerceDemo.IdentityModule.Application.Tests.Features.AspNetUserLogin
{
    public class InsertAspNetUserLoginCommandTests : IDisposable
    {
        private const int ResultCodeSuccess = 0;
        private const int ResultCodeNotFound = 404;
        private readonly Mock<IAspNetUserLoginRepository> _repositoryMock;
        private readonly Mock<ILogger<InsertAspNetUserLoginCommand.InsertAspNetUserLoginHandler>> _loggerMock;
        public InsertAspNetUserLoginCommandTests()
        {
            _repositoryMock = new Mock<IAspNetUserLoginRepository>();
            _loggerMock = new Mock<ILogger<InsertAspNetUserLoginCommand.InsertAspNetUserLoginHandler>>();
        }

        [Fact]
        public async Task Handle_Should_Return_Success_When_Valid_Command()
        {
            // Arrange
            var fakeDto = TestDataGenerator.CreateFake<AspNetUserLoginDto>("tr");
            var fakeResponse = new RepoResponse<AspNetUserLoginDto>(fakeDto, "Success");
            _repositoryMock.Setup(r => r.InsertAsync(It.IsAny<AspNetUserLoginDto>())).ReturnsAsync(fakeResponse);
            var handler = new InsertAspNetUserLoginCommand.InsertAspNetUserLoginHandler(_loggerMock.Object, _repositoryMock.Object);
            var command = new InsertAspNetUserLoginCommand(fakeDto);
            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            // Assert
            Assert.Equal(ResultCodeSuccess, result.Code);
            Assert.Equal(fakeDto, result.Value);
            _repositoryMock.Verify(r => r.InsertAsync(It.IsAny<AspNetUserLoginDto>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_NotFound_When_Repository_Returns_404()
        {
            // Arrange
            var fakeDto = TestDataGenerator.CreateFake<AspNetUserLoginDto>("tr");
            var fakeResponse = new RepoResponse<AspNetUserLoginDto>
            {
                Code = ResultCodeNotFound,
                Message = "Not found"
            };
            _repositoryMock.Setup(r => r.InsertAsync(It.IsAny<AspNetUserLoginDto>())).ReturnsAsync(fakeResponse);
            var handler = new InsertAspNetUserLoginCommand.InsertAspNetUserLoginHandler(_loggerMock.Object, _repositoryMock.Object);
            var command = new InsertAspNetUserLoginCommand(fakeDto);
            // Act
            var result = await handler.Handle(command, CancellationToken.None);
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