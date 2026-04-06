using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Features.KafkaEvent;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos.KafkaEvent;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Interfaces.Repositories;
using QuickCode.MyecommerceDemo.Common.Helpers;
using QuickCode.MyecommerceDemo.Common.Models;

namespace QuickCode.MyecommerceDemo.IdentityModule.Application.Tests.Features.KafkaEvent
{
    public class DeleteItemKafkaEventCommandTests : IDisposable
    {
        private const int ResultCodeSuccess = 0;
        private const int ResultCodeNotFound = 404;
        private readonly Mock<IKafkaEventRepository> _repositoryMock;
        private readonly Mock<ILogger<DeleteItemKafkaEventCommand.DeleteItemKafkaEventHandler>> _loggerMock;
        public DeleteItemKafkaEventCommandTests()
        {
            _repositoryMock = new Mock<IKafkaEventRepository>();
            _loggerMock = new Mock<ILogger<DeleteItemKafkaEventCommand.DeleteItemKafkaEventHandler>>();
        }

        [Fact]
        public async Task Handle_Should_Return_Success_When_Valid_Command()
        {
            // Arrange
            var fakeDto = TestDataGenerator.CreateFake<KafkaEventDto>("tr");
            var fakeResponse = new RepoResponse<bool>(true, "Success");
            _repositoryMock.Setup(r => r.DeleteAsync(It.IsAny<KafkaEventDto>())).ReturnsAsync(fakeResponse);
            _repositoryMock.Setup(r => r.GetByPkAsync(fakeDto.TopicName)).ReturnsAsync(new RepoResponse<KafkaEventDto>());
            var handler = new DeleteItemKafkaEventCommand.DeleteItemKafkaEventHandler(_loggerMock.Object, _repositoryMock.Object);
            var command = new DeleteItemKafkaEventCommand(fakeDto.TopicName);
            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            // Assert
            Assert.Equal(ResultCodeSuccess, result.Code);
            Assert.True(result.Value);
            _repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<KafkaEventDto>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_NotFound_When_Repository_Returns_404()
        {
            // Arrange
            var fakeDto = TestDataGenerator.CreateFake<KafkaEventDto>("tr");
            var fakeResponse = new RepoResponse<bool>
            {
                Code = ResultCodeNotFound,
                Message = "Not found",
                Value = false
            };
            _repositoryMock.Setup(r => r.DeleteAsync(It.IsAny<KafkaEventDto>())).ReturnsAsync(fakeResponse);
            _repositoryMock.Setup(r => r.GetByPkAsync(fakeDto.TopicName)).ReturnsAsync(new RepoResponse<KafkaEventDto>());
            var handler = new DeleteItemKafkaEventCommand.DeleteItemKafkaEventHandler(_loggerMock.Object, _repositoryMock.Object);
            var command = new DeleteItemKafkaEventCommand(fakeDto.TopicName);
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