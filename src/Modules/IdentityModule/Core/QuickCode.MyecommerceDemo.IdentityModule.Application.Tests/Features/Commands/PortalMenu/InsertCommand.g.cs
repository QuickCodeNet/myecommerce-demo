using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Features.PortalMenu;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos.PortalMenu;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Interfaces.Repositories;
using QuickCode.MyecommerceDemo.Common.Helpers;
using QuickCode.MyecommerceDemo.Common.Models;

namespace QuickCode.MyecommerceDemo.IdentityModule.Application.Tests.Features.PortalMenu
{
    public class InsertPortalMenuCommandTests : IDisposable
    {
        private const int ResultCodeSuccess = 0;
        private const int ResultCodeNotFound = 404;
        private readonly Mock<IPortalMenuRepository> _repositoryMock;
        private readonly Mock<ILogger<InsertPortalMenuCommand.InsertPortalMenuHandler>> _loggerMock;
        public InsertPortalMenuCommandTests()
        {
            _repositoryMock = new Mock<IPortalMenuRepository>();
            _loggerMock = new Mock<ILogger<InsertPortalMenuCommand.InsertPortalMenuHandler>>();
        }

        [Fact]
        public async Task Handle_Should_Return_Success_When_Valid_Command()
        {
            // Arrange
            var fakeDto = TestDataGenerator.CreateFake<PortalMenuDto>("tr");
            var fakeResponse = new RepoResponse<PortalMenuDto>(fakeDto, "Success");
            _repositoryMock.Setup(r => r.InsertAsync(It.IsAny<PortalMenuDto>())).ReturnsAsync(fakeResponse);
            var handler = new InsertPortalMenuCommand.InsertPortalMenuHandler(_loggerMock.Object, _repositoryMock.Object);
            var command = new InsertPortalMenuCommand(fakeDto);
            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            // Assert
            Assert.Equal(ResultCodeSuccess, result.Code);
            Assert.Equal(fakeDto, result.Value);
            _repositoryMock.Verify(r => r.InsertAsync(It.IsAny<PortalMenuDto>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_NotFound_When_Repository_Returns_404()
        {
            // Arrange
            var fakeDto = TestDataGenerator.CreateFake<PortalMenuDto>("tr");
            var fakeResponse = new RepoResponse<PortalMenuDto>
            {
                Code = ResultCodeNotFound,
                Message = "Not found"
            };
            _repositoryMock.Setup(r => r.InsertAsync(It.IsAny<PortalMenuDto>())).ReturnsAsync(fakeResponse);
            var handler = new InsertPortalMenuCommand.InsertPortalMenuHandler(_loggerMock.Object, _repositoryMock.Object);
            var command = new InsertPortalMenuCommand(fakeDto);
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