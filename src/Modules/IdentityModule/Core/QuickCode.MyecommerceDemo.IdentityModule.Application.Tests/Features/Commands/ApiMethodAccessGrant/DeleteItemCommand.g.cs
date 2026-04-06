using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Features.ApiMethodAccessGrant;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos.ApiMethodAccessGrant;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Interfaces.Repositories;
using QuickCode.MyecommerceDemo.Common.Helpers;
using QuickCode.MyecommerceDemo.Common.Models;

namespace QuickCode.MyecommerceDemo.IdentityModule.Application.Tests.Features.ApiMethodAccessGrant
{
    public class DeleteItemApiMethodAccessGrantCommandTests : IDisposable
    {
        private const int ResultCodeSuccess = 0;
        private const int ResultCodeNotFound = 404;
        private readonly Mock<IApiMethodAccessGrantRepository> _repositoryMock;
        private readonly Mock<ILogger<DeleteItemApiMethodAccessGrantCommand.DeleteItemApiMethodAccessGrantHandler>> _loggerMock;
        public DeleteItemApiMethodAccessGrantCommandTests()
        {
            _repositoryMock = new Mock<IApiMethodAccessGrantRepository>();
            _loggerMock = new Mock<ILogger<DeleteItemApiMethodAccessGrantCommand.DeleteItemApiMethodAccessGrantHandler>>();
        }

        [Fact]
        public async Task Handle_Should_Return_Success_When_Valid_Command()
        {
            // Arrange
            var fakeDto = TestDataGenerator.CreateFake<ApiMethodAccessGrantDto>("tr");
            var fakeResponse = new RepoResponse<bool>(true, "Success");
            _repositoryMock.Setup(r => r.DeleteAsync(It.IsAny<ApiMethodAccessGrantDto>())).ReturnsAsync(fakeResponse);
            _repositoryMock.Setup(r => r.GetByPkAsync(fakeDto.PermissionGroupName, fakeDto.ApiMethodDefinitionKey)).ReturnsAsync(new RepoResponse<ApiMethodAccessGrantDto>());
            var handler = new DeleteItemApiMethodAccessGrantCommand.DeleteItemApiMethodAccessGrantHandler(_loggerMock.Object, _repositoryMock.Object);
            var command = new DeleteItemApiMethodAccessGrantCommand(fakeDto.PermissionGroupName, fakeDto.ApiMethodDefinitionKey);
            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            // Assert
            Assert.Equal(ResultCodeSuccess, result.Code);
            Assert.True(result.Value);
            _repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<ApiMethodAccessGrantDto>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_NotFound_When_Repository_Returns_404()
        {
            // Arrange
            var fakeDto = TestDataGenerator.CreateFake<ApiMethodAccessGrantDto>("tr");
            var fakeResponse = new RepoResponse<bool>
            {
                Code = ResultCodeNotFound,
                Message = "Not found",
                Value = false
            };
            _repositoryMock.Setup(r => r.DeleteAsync(It.IsAny<ApiMethodAccessGrantDto>())).ReturnsAsync(fakeResponse);
            _repositoryMock.Setup(r => r.GetByPkAsync(fakeDto.PermissionGroupName, fakeDto.ApiMethodDefinitionKey)).ReturnsAsync(new RepoResponse<ApiMethodAccessGrantDto>());
            var handler = new DeleteItemApiMethodAccessGrantCommand.DeleteItemApiMethodAccessGrantHandler(_loggerMock.Object, _repositoryMock.Object);
            var command = new DeleteItemApiMethodAccessGrantCommand(fakeDto.PermissionGroupName, fakeDto.ApiMethodDefinitionKey);
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