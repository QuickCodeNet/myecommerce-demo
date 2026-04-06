using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Features.PortalPageAccessGrant;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos.PortalPageAccessGrant;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Interfaces.Repositories;
using QuickCode.MyecommerceDemo.Common.Helpers;
using QuickCode.MyecommerceDemo.Common.Models;

namespace QuickCode.MyecommerceDemo.IdentityModule.Application.Tests.Features.PortalPageAccessGrant
{
    public class InsertPortalPageAccessGrantCommandTests : IDisposable
    {
        private const int ResultCodeSuccess = 0;
        private const int ResultCodeNotFound = 404;
        private readonly Mock<IPortalPageAccessGrantRepository> _repositoryMock;
        private readonly Mock<ILogger<InsertPortalPageAccessGrantCommand.InsertPortalPageAccessGrantHandler>> _loggerMock;
        public InsertPortalPageAccessGrantCommandTests()
        {
            _repositoryMock = new Mock<IPortalPageAccessGrantRepository>();
            _loggerMock = new Mock<ILogger<InsertPortalPageAccessGrantCommand.InsertPortalPageAccessGrantHandler>>();
        }

        [Fact]
        public async Task Handle_Should_Return_Success_When_Valid_Command()
        {
            // Arrange
            var fakeDto = TestDataGenerator.CreateFake<PortalPageAccessGrantDto>("tr");
            var fakeResponse = new RepoResponse<PortalPageAccessGrantDto>(fakeDto, "Success");
            _repositoryMock.Setup(r => r.InsertAsync(It.IsAny<PortalPageAccessGrantDto>())).ReturnsAsync(fakeResponse);
            var handler = new InsertPortalPageAccessGrantCommand.InsertPortalPageAccessGrantHandler(_loggerMock.Object, _repositoryMock.Object);
            var command = new InsertPortalPageAccessGrantCommand(fakeDto);
            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            // Assert
            Assert.Equal(ResultCodeSuccess, result.Code);
            Assert.Equal(fakeDto, result.Value);
            _repositoryMock.Verify(r => r.InsertAsync(It.IsAny<PortalPageAccessGrantDto>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_NotFound_When_Repository_Returns_404()
        {
            // Arrange
            var fakeDto = TestDataGenerator.CreateFake<PortalPageAccessGrantDto>("tr");
            var fakeResponse = new RepoResponse<PortalPageAccessGrantDto>
            {
                Code = ResultCodeNotFound,
                Message = "Not found"
            };
            _repositoryMock.Setup(r => r.InsertAsync(It.IsAny<PortalPageAccessGrantDto>())).ReturnsAsync(fakeResponse);
            var handler = new InsertPortalPageAccessGrantCommand.InsertPortalPageAccessGrantHandler(_loggerMock.Object, _repositoryMock.Object);
            var command = new InsertPortalPageAccessGrantCommand(fakeDto);
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