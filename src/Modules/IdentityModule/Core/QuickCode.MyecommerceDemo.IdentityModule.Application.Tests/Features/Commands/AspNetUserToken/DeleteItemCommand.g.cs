using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Features.AspNetUserToken;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos.AspNetUserToken;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Interfaces.Repositories;
using QuickCode.MyecommerceDemo.Common.Helpers;
using QuickCode.MyecommerceDemo.Common.Models;

namespace QuickCode.MyecommerceDemo.IdentityModule.Application.Tests.Features.AspNetUserToken
{
    public class DeleteItemAspNetUserTokenCommandTests : IDisposable
    {
        private const int ResultCodeSuccess = 0;
        private const int ResultCodeNotFound = 404;
        private readonly Mock<IAspNetUserTokenRepository> _repositoryMock;
        private readonly Mock<ILogger<DeleteItemAspNetUserTokenCommand.DeleteItemAspNetUserTokenHandler>> _loggerMock;
        public DeleteItemAspNetUserTokenCommandTests()
        {
            _repositoryMock = new Mock<IAspNetUserTokenRepository>();
            _loggerMock = new Mock<ILogger<DeleteItemAspNetUserTokenCommand.DeleteItemAspNetUserTokenHandler>>();
        }

        [Fact]
        public async Task Handle_Should_Return_Success_When_Valid_Command()
        {
            // Arrange
            var fakeDto = TestDataGenerator.CreateFake<AspNetUserTokenDto>("tr");
            var fakeResponse = new RepoResponse<bool>(true, "Success");
            _repositoryMock.Setup(r => r.DeleteAsync(It.IsAny<AspNetUserTokenDto>())).ReturnsAsync(fakeResponse);
            _repositoryMock.Setup(r => r.GetByPkAsync(fakeDto.UserId)).ReturnsAsync(new RepoResponse<AspNetUserTokenDto>());
            var handler = new DeleteItemAspNetUserTokenCommand.DeleteItemAspNetUserTokenHandler(_loggerMock.Object, _repositoryMock.Object);
            var command = new DeleteItemAspNetUserTokenCommand(fakeDto.UserId);
            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            // Assert
            Assert.Equal(ResultCodeSuccess, result.Code);
            Assert.True(result.Value);
            _repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<AspNetUserTokenDto>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_NotFound_When_Repository_Returns_404()
        {
            // Arrange
            var fakeDto = TestDataGenerator.CreateFake<AspNetUserTokenDto>("tr");
            var fakeResponse = new RepoResponse<bool>
            {
                Code = ResultCodeNotFound,
                Message = "Not found",
                Value = false
            };
            _repositoryMock.Setup(r => r.DeleteAsync(It.IsAny<AspNetUserTokenDto>())).ReturnsAsync(fakeResponse);
            _repositoryMock.Setup(r => r.GetByPkAsync(fakeDto.UserId)).ReturnsAsync(new RepoResponse<AspNetUserTokenDto>());
            var handler = new DeleteItemAspNetUserTokenCommand.DeleteItemAspNetUserTokenHandler(_loggerMock.Object, _repositoryMock.Object);
            var command = new DeleteItemAspNetUserTokenCommand(fakeDto.UserId);
            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            // Assert
            Assert.Equal(ResultCodeNotFound, result.Code);
            Assert.False(result.Value);
        }

        public void Dispose()
        {
        // Cleanup handled by xUnit
        }
    }
}