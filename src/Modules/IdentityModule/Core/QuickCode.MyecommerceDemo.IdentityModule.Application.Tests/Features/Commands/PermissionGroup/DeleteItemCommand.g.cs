using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Features.PermissionGroup;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos.PermissionGroup;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Interfaces.Repositories;
using QuickCode.MyecommerceDemo.Common.Helpers;
using QuickCode.MyecommerceDemo.Common.Models;

namespace QuickCode.MyecommerceDemo.IdentityModule.Application.Tests.Features.PermissionGroup
{
    public class DeleteItemPermissionGroupCommandTests : IDisposable
    {
        private const int ResultCodeSuccess = 0;
        private const int ResultCodeNotFound = 404;
        private readonly Mock<IPermissionGroupRepository> _repositoryMock;
        private readonly Mock<ILogger<DeleteItemPermissionGroupCommand.DeleteItemPermissionGroupHandler>> _loggerMock;
        public DeleteItemPermissionGroupCommandTests()
        {
            _repositoryMock = new Mock<IPermissionGroupRepository>();
            _loggerMock = new Mock<ILogger<DeleteItemPermissionGroupCommand.DeleteItemPermissionGroupHandler>>();
        }

        [Fact]
        public async Task Handle_Should_Return_Success_When_Valid_Command()
        {
            // Arrange
            var fakeDto = TestDataGenerator.CreateFake<PermissionGroupDto>("tr");
            var fakeResponse = new RepoResponse<bool>(true, "Success");
            _repositoryMock.Setup(r => r.DeleteAsync(It.IsAny<PermissionGroupDto>())).ReturnsAsync(fakeResponse);
            _repositoryMock.Setup(r => r.GetByPkAsync(fakeDto.Name)).ReturnsAsync(new RepoResponse<PermissionGroupDto>());
            var handler = new DeleteItemPermissionGroupCommand.DeleteItemPermissionGroupHandler(_loggerMock.Object, _repositoryMock.Object);
            var command = new DeleteItemPermissionGroupCommand(fakeDto.Name);
            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            // Assert
            Assert.Equal(ResultCodeSuccess, result.Code);
            Assert.True(result.Value);
            _repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<PermissionGroupDto>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_NotFound_When_Repository_Returns_404()
        {
            // Arrange
            var fakeDto = TestDataGenerator.CreateFake<PermissionGroupDto>("tr");
            var fakeResponse = new RepoResponse<bool>
            {
                Code = ResultCodeNotFound,
                Message = "Not found",
                Value = false
            };
            _repositoryMock.Setup(r => r.DeleteAsync(It.IsAny<PermissionGroupDto>())).ReturnsAsync(fakeResponse);
            _repositoryMock.Setup(r => r.GetByPkAsync(fakeDto.Name)).ReturnsAsync(new RepoResponse<PermissionGroupDto>());
            var handler = new DeleteItemPermissionGroupCommand.DeleteItemPermissionGroupHandler(_loggerMock.Object, _repositoryMock.Object);
            var command = new DeleteItemPermissionGroupCommand(fakeDto.Name);
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