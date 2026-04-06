using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Features.ColumnType;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos.ColumnType;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Interfaces.Repositories;
using QuickCode.MyecommerceDemo.Common.Helpers;
using QuickCode.MyecommerceDemo.Common.Models;

namespace QuickCode.MyecommerceDemo.IdentityModule.Application.Tests.Features.ColumnType
{
    public class InsertColumnTypeCommandTests : IDisposable
    {
        private const int ResultCodeSuccess = 0;
        private const int ResultCodeNotFound = 404;
        private readonly Mock<IColumnTypeRepository> _repositoryMock;
        private readonly Mock<ILogger<InsertColumnTypeCommand.InsertColumnTypeHandler>> _loggerMock;
        public InsertColumnTypeCommandTests()
        {
            _repositoryMock = new Mock<IColumnTypeRepository>();
            _loggerMock = new Mock<ILogger<InsertColumnTypeCommand.InsertColumnTypeHandler>>();
        }

        [Fact]
        public async Task Handle_Should_Return_Success_When_Valid_Command()
        {
            // Arrange
            var fakeDto = TestDataGenerator.CreateFake<ColumnTypeDto>("tr");
            var fakeResponse = new RepoResponse<ColumnTypeDto>(fakeDto, "Success");
            _repositoryMock.Setup(r => r.InsertAsync(It.IsAny<ColumnTypeDto>())).ReturnsAsync(fakeResponse);
            var handler = new InsertColumnTypeCommand.InsertColumnTypeHandler(_loggerMock.Object, _repositoryMock.Object);
            var command = new InsertColumnTypeCommand(fakeDto);
            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            // Assert
            Assert.Equal(ResultCodeSuccess, result.Code);
            Assert.Equal(fakeDto, result.Value);
            _repositoryMock.Verify(r => r.InsertAsync(It.IsAny<ColumnTypeDto>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_NotFound_When_Repository_Returns_404()
        {
            // Arrange
            var fakeDto = TestDataGenerator.CreateFake<ColumnTypeDto>("tr");
            var fakeResponse = new RepoResponse<ColumnTypeDto>
            {
                Code = ResultCodeNotFound,
                Message = "Not found"
            };
            _repositoryMock.Setup(r => r.InsertAsync(It.IsAny<ColumnTypeDto>())).ReturnsAsync(fakeResponse);
            var handler = new InsertColumnTypeCommand.InsertColumnTypeHandler(_loggerMock.Object, _repositoryMock.Object);
            var command = new InsertColumnTypeCommand(fakeDto);
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