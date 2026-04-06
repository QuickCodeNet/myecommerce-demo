using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Features.TopicWorkflow;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos.TopicWorkflow;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Interfaces.Repositories;
using QuickCode.MyecommerceDemo.Common.Helpers;
using QuickCode.MyecommerceDemo.Common.Models;

namespace QuickCode.MyecommerceDemo.IdentityModule.Application.Tests.Features.TopicWorkflow
{
    public class InsertTopicWorkflowCommandTests : IDisposable
    {
        private const int ResultCodeSuccess = 0;
        private const int ResultCodeNotFound = 404;
        private readonly Mock<ITopicWorkflowRepository> _repositoryMock;
        private readonly Mock<ILogger<InsertTopicWorkflowCommand.InsertTopicWorkflowHandler>> _loggerMock;
        public InsertTopicWorkflowCommandTests()
        {
            _repositoryMock = new Mock<ITopicWorkflowRepository>();
            _loggerMock = new Mock<ILogger<InsertTopicWorkflowCommand.InsertTopicWorkflowHandler>>();
        }

        [Fact]
        public async Task Handle_Should_Return_Success_When_Valid_Command()
        {
            // Arrange
            var fakeDto = TestDataGenerator.CreateFake<TopicWorkflowDto>("tr");
            var fakeResponse = new RepoResponse<TopicWorkflowDto>(fakeDto, "Success");
            _repositoryMock.Setup(r => r.InsertAsync(It.IsAny<TopicWorkflowDto>())).ReturnsAsync(fakeResponse);
            var handler = new InsertTopicWorkflowCommand.InsertTopicWorkflowHandler(_loggerMock.Object, _repositoryMock.Object);
            var command = new InsertTopicWorkflowCommand(fakeDto);
            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            // Assert
            Assert.Equal(ResultCodeSuccess, result.Code);
            Assert.Equal(fakeDto, result.Value);
            _repositoryMock.Verify(r => r.InsertAsync(It.IsAny<TopicWorkflowDto>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_NotFound_When_Repository_Returns_404()
        {
            // Arrange
            var fakeDto = TestDataGenerator.CreateFake<TopicWorkflowDto>("tr");
            var fakeResponse = new RepoResponse<TopicWorkflowDto>
            {
                Code = ResultCodeNotFound,
                Message = "Not found"
            };
            _repositoryMock.Setup(r => r.InsertAsync(It.IsAny<TopicWorkflowDto>())).ReturnsAsync(fakeResponse);
            var handler = new InsertTopicWorkflowCommand.InsertTopicWorkflowHandler(_loggerMock.Object, _repositoryMock.Object);
            var command = new InsertTopicWorkflowCommand(fakeDto);
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