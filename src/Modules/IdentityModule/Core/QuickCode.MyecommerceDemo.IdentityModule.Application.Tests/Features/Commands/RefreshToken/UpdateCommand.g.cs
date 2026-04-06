using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Features.RefreshToken;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos.RefreshToken;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Interfaces.Repositories;
using QuickCode.MyecommerceDemo.Common.Helpers;
using QuickCode.MyecommerceDemo.Common.Models;

namespace QuickCode.MyecommerceDemo.IdentityModule.Application.Tests.Features.RefreshToken
{
    public class UpdateRefreshTokenCommandTests : IDisposable
    {
        private const int ResultCodeSuccess = 0;
        private const int ResultCodeNotFound = 404;
        private readonly Mock<IRefreshTokenRepository> _repositoryMock;
        private readonly Mock<ILogger<UpdateRefreshTokenCommand.UpdateRefreshTokenHandler>> _loggerMock;
        public UpdateRefreshTokenCommandTests()
        {
            _repositoryMock = new Mock<IRefreshTokenRepository>();
            _loggerMock = new Mock<ILogger<UpdateRefreshTokenCommand.UpdateRefreshTokenHandler>>();
        }

        [Fact]
        public async Task Handle_Should_Return_Success_When_Valid_Command()
        {
            // Arrange
            var fakeDto = TestDataGenerator.CreateFake<RefreshTokenDto>("tr");
            var fakeResponse = new RepoResponse<bool>(true, "Success");
            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<RefreshTokenDto>())).ReturnsAsync(fakeResponse);
            _repositoryMock.Setup(r => r.GetByPkAsync(fakeDto.Id)).ReturnsAsync(new RepoResponse<RefreshTokenDto>());
            var handler = new UpdateRefreshTokenCommand.UpdateRefreshTokenHandler(_loggerMock.Object, _repositoryMock.Object);
            var command = new UpdateRefreshTokenCommand(fakeDto.Id, fakeDto);
            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            // Assert
            Assert.Equal(ResultCodeSuccess, result.Code);
            Assert.True(result.Value);
            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<RefreshTokenDto>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_NotFound_When_Repository_Returns_404()
        {
            // Arrange
            var fakeDto = TestDataGenerator.CreateFake<RefreshTokenDto>("tr");
            var fakeResponse = new RepoResponse<bool>
            {
                Code = ResultCodeNotFound,
                Message = "Not found"
            };
            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<RefreshTokenDto>())).ReturnsAsync(fakeResponse);
            _repositoryMock.Setup(r => r.GetByPkAsync(fakeDto.Id)).ReturnsAsync(new RepoResponse<RefreshTokenDto>());
            var handler = new UpdateRefreshTokenCommand.UpdateRefreshTokenHandler(_loggerMock.Object, _repositoryMock.Object);
            var command = new UpdateRefreshTokenCommand(fakeDto.Id, fakeDto);
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