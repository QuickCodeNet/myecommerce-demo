using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Features.AspNetUserClaim;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos.AspNetUserClaim;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Interfaces.Repositories;
using QuickCode.MyecommerceDemo.Common.Helpers;
using QuickCode.MyecommerceDemo.Common.Models;

namespace QuickCode.MyecommerceDemo.IdentityModule.Application.Tests.Features.AspNetUserClaim
{
    public class DeleteItemAspNetUserClaimCommandTests : IDisposable
    {
        private const int ResultCodeSuccess = 0;
        private const int ResultCodeNotFound = 404;
        private readonly Mock<IAspNetUserClaimRepository> _repositoryMock;
        private readonly Mock<ILogger<DeleteItemAspNetUserClaimCommand.DeleteItemAspNetUserClaimHandler>> _loggerMock;
        public DeleteItemAspNetUserClaimCommandTests()
        {
            _repositoryMock = new Mock<IAspNetUserClaimRepository>();
            _loggerMock = new Mock<ILogger<DeleteItemAspNetUserClaimCommand.DeleteItemAspNetUserClaimHandler>>();
        }

        [Fact]
        public async Task Handle_Should_Return_Success_When_Valid_Command()
        {
            // Arrange
            var fakeDto = TestDataGenerator.CreateFake<AspNetUserClaimDto>("tr");
            var fakeResponse = new RepoResponse<bool>(true, "Success");
            _repositoryMock.Setup(r => r.DeleteAsync(It.IsAny<AspNetUserClaimDto>())).ReturnsAsync(fakeResponse);
            _repositoryMock.Setup(r => r.GetByPkAsync(fakeDto.Id)).ReturnsAsync(new RepoResponse<AspNetUserClaimDto>());
            var handler = new DeleteItemAspNetUserClaimCommand.DeleteItemAspNetUserClaimHandler(_loggerMock.Object, _repositoryMock.Object);
            var command = new DeleteItemAspNetUserClaimCommand(fakeDto.Id);
            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            // Assert
            Assert.Equal(ResultCodeSuccess, result.Code);
            Assert.True(result.Value);
            _repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<AspNetUserClaimDto>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_NotFound_When_Repository_Returns_404()
        {
            // Arrange
            var fakeDto = TestDataGenerator.CreateFake<AspNetUserClaimDto>("tr");
            var fakeResponse = new RepoResponse<bool>
            {
                Code = ResultCodeNotFound,
                Message = "Not found",
                Value = false
            };
            _repositoryMock.Setup(r => r.DeleteAsync(It.IsAny<AspNetUserClaimDto>())).ReturnsAsync(fakeResponse);
            _repositoryMock.Setup(r => r.GetByPkAsync(fakeDto.Id)).ReturnsAsync(new RepoResponse<AspNetUserClaimDto>());
            var handler = new DeleteItemAspNetUserClaimCommand.DeleteItemAspNetUserClaimHandler(_loggerMock.Object, _repositoryMock.Object);
            var command = new DeleteItemAspNetUserClaimCommand(fakeDto.Id);
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