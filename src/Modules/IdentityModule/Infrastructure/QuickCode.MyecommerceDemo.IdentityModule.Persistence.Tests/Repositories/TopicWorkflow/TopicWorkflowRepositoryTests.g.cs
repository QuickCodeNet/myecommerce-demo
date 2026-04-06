using System;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using QuickCode.MyecommerceDemo.IdentityModule.Persistence.Repositories;
using QuickCode.MyecommerceDemo.IdentityModule.Persistence.Contexts;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos.TopicWorkflow;
using QuickCode.MyecommerceDemo.Common.Helpers;
using Xunit.Abstractions;

namespace QuickCode.MyecommerceDemo.IdentityModule.Persistence.Tests.Repositories
{
    public class TopicWorkflowRepositoryTests : IDisposable
    {
        private const int ResultCodeSuccess = 0;
        private const int ResultCodeNotFound = 404;
        private DbContextOptions<WriteDbContext> writeOptions = null;
        private DbContextOptions<ReadDbContext> readOptions = null;
        private Mock<ILogger<TopicWorkflowRepository>> loggerMock = null;
        private readonly ITestOutputHelper _output;
        public TopicWorkflowRepositoryTests(ITestOutputHelper output)
        {
            _output = output;
            CreateDatabase();
        }

        private TopicWorkflowRepository CreateRepository()
        {
            var writeContext = new WriteDbContext(writeOptions);
            var readContext = new ReadDbContext(readOptions);
            return new TopicWorkflowRepository(loggerMock.Object, writeContext, readContext);
        }

        private void CreateDatabase()
        {
            var databaseName = $"TestDb_{Guid.NewGuid()}";
            writeOptions = new DbContextOptionsBuilder<WriteDbContext>().UseInMemoryDatabase(databaseName).Options;
            readOptions = new DbContextOptionsBuilder<ReadDbContext>().UseInMemoryDatabase(databaseName).Options;
            loggerMock = new Mock<ILogger<TopicWorkflowRepository>>();
        }

        [Fact]
        public async Task InsertAsync_Should_Insert_Item_Successfully()
        {
            // Arrange
            var repository = CreateRepository();
            var itemDto = TestDataGenerator.CreateFake<TopicWorkflowDto>("tr");
            // Act
            var result = await repository.InsertAsync(itemDto);
            // Assert
            Assert.Equal(ResultCodeSuccess, result.Code);
            itemDto.AssertPropertiesEqual(result.Value);
        }

        [Fact]
        public async Task InsertAsync_Should_Generate_Unique_Ids()
        {
            // Arrange
            var repository = CreateRepository();
            var fakeItem1 = TestDataGenerator.CreateFake<TopicWorkflowDto>();
            var fakeItem2 = TestDataGenerator.CreateFake<TopicWorkflowDto>("tr");
            // Act
            var result1 = await repository.InsertAsync(fakeItem1);
            var result2 = await repository.InsertAsync(fakeItem2);
            // Assert
            Assert.Equal(ResultCodeSuccess, result1.Code);
            Assert.Equal(ResultCodeSuccess, result2.Code);
            Assert.NotEqual(result1.Value.Id, result2.Value.Id);
        }

        [Fact]
        public async Task GetByPkAsync_Should_Return_Item_When_Exists()
        {
            // Arrange
            var repository = CreateRepository();
            var itemDto = TestDataGenerator.CreateFake<TopicWorkflowDto>();
            var insertedItem = await repository.InsertAsync(itemDto);
            Assert.Equal(ResultCodeSuccess, insertedItem.Code);
            // Act
            var result = await repository.GetByPkAsync(insertedItem.Value.Id);
            // Assert
            Assert.Equal(ResultCodeSuccess, result.Code);
            itemDto.AssertPropertiesEqual(result.Value);
        }

        [Fact]
        public async Task GetByPkAsync_Should_Return_NotFound_When_Item_Not_Exists()
        {
            // Arrange
            var repository = CreateRepository();
            var notExistsItem = TestDataGenerator.CreateFake<TopicWorkflowDto>("tr");
            // Act
            var result = await repository.GetByPkAsync(notExistsItem.Id);
            // Assert
            Assert.Equal(ResultCodeNotFound, result.Code);
            Assert.Equal("Not found in TopicWorkflow", result.Message);
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_Item_Successfully()
        {
            // Arrange
            var repository = CreateRepository();
            var itemDto = TestDataGenerator.CreateFake<TopicWorkflowDto>("tr");
            var insertResult = await repository.InsertAsync(itemDto);
            Assert.Equal(ResultCodeSuccess, insertResult.Code);
            var insertedItem = insertResult.Value;
            // Update the item using with expression
            var updates = insertedItem.GenerateRandomUpdates(["Id"]);
            var updateItemDto = insertedItem.UpdateProperties(updates);
            repository = CreateRepository();
            // Act
            var result = await repository.UpdateAsync(updateItemDto);
            // Assert
            Assert.Equal(ResultCodeSuccess, result.Code);
            Assert.True(result.Value);
            // Verify the update directly from writeContext
            var updatedItem = await repository.GetByPkAsync(insertedItem.Id);
            Assert.Equal(ResultCodeSuccess, updatedItem.Code);
            foreach (var update in updates)
            {
                var property = updatedItem.Value.GetType().GetProperty(update.Key);
                Assert.NotNull(property);
                Assert.Equal(update.Value, property.GetValue(updatedItem.Value));
            }
        }

        [Fact]
        public async Task DeleteAsync_Should_Delete_Item_Successfully()
        {
            // Arrange
            var repository = CreateRepository();
            var itemDto = TestDataGenerator.CreateFake<TopicWorkflowDto>();
            var insertedItem = await repository.InsertAsync(itemDto);
            Assert.Equal(ResultCodeSuccess, insertedItem.Code);
            repository = CreateRepository();
            // Act
            var result = await repository.DeleteAsync(insertedItem.Value);
            // Assert
            Assert.Equal(ResultCodeSuccess, result.Code);
            Assert.True(result.Value);
            // Verify deletion directly from writeContext
            var deletedItem = await repository.GetByPkAsync(insertedItem.Value.Id);
            Assert.Equal(ResultCodeNotFound, deletedItem.Code);
        }

        [Fact]
        public async Task ListAsync_Should_Return_All_TopicWorkflow()
        {
            // Arrange
            CreateDatabase();
            var repository = CreateRepository();
            var fakeItems = TestDataGenerator.CreateFakes<TopicWorkflowDto>("tr");
            foreach (var fakeItem in fakeItems)
            {
                var insertResponse = await repository.InsertAsync(fakeItem);
                if (insertResponse.Code != ResultCodeSuccess)
                {
                    _output.WriteLine(insertResponse.Message);
                }

                Assert.Equal(ResultCodeSuccess, insertResponse.Code);
            }

            // Act
            var result = await repository.ListAsync();
            // Assert
            Assert.Equal(ResultCodeSuccess, result.Code);
            Assert.Equal(3, result.Value.Count);
        }

        [Fact]
        public async Task ListAsync_With_Pagination_Should_Return_Correct_Page()
        {
            // Arrange
            var repository = CreateRepository();
            var fakeItems = TestDataGenerator.CreateFakes<TopicWorkflowDto>("en", 5);
            foreach (var fakeItem in fakeItems)
            {
                var insertResponse = await repository.InsertAsync(fakeItem);
                if (insertResponse.Code != ResultCodeSuccess)
                {
                    _output.WriteLine(insertResponse.Message);
                }

                Assert.Equal(ResultCodeSuccess, insertResponse.Code);
            }

            // Act
            var result = await repository.ListAsync(pageNumber: 1, pageSize: 2);
            // Assert
            Assert.Equal(ResultCodeSuccess, result.Code);
            Assert.Equal(2, result.Value.Count);
        }

        [Fact]
        public async Task ListAsync_With_PageNumber_Less_Than_One_Should_Return_Error()
        {
            // Arrange
            var repository = CreateRepository();
            // Act
            var result = await repository.ListAsync(pageNumber: 0, pageSize: 10);
            // Assert
            Assert.Equal(ResultCodeNotFound, result.Code);
            Assert.Contains("Page Number must be greater than 1", result.Message);
        }

        [Fact]
        public async Task ListAsync_With_Null_Pagination_Should_Return_All_Records()
        {
            // Arrange
            CreateDatabase();
            var repository = CreateRepository();
            var fakeItems = TestDataGenerator.CreateFakes<TopicWorkflowDto>("tr");
            foreach (var fakeItem in fakeItems)
            {
                var insertResponse = await repository.InsertAsync(fakeItem);
                if (insertResponse.Code != ResultCodeSuccess)
                {
                    _output.WriteLine(insertResponse.Message);
                }

                Assert.Equal(ResultCodeSuccess, insertResponse.Code);
            }

            // Act
            var result = await repository.ListAsync(pageNumber: null, pageSize: null);
            // Assert
            Assert.Equal(ResultCodeSuccess, result.Code);
            Assert.Equal(3, result.Value.Count);
        }

        [Fact]
        public async Task CountAsync_Should_Return_Correct_Count()
        {
            // Arrange
            CreateDatabase();
            var repository = CreateRepository();
            var fakeItems = TestDataGenerator.CreateFakes<TopicWorkflowDto>("tr");
            foreach (var fakeItem in fakeItems)
            {
                var insertResponse = await repository.InsertAsync(fakeItem);
                if (insertResponse.Code != ResultCodeSuccess)
                {
                    _output.WriteLine(insertResponse.Message);
                }

                Assert.Equal(ResultCodeSuccess, insertResponse.Code);
            }

            // Act
            var result = await repository.CountAsync();
            // Assert
            Assert.Equal(ResultCodeSuccess, result.Code);
            Assert.Equal(3, result.Value);
        }

        [Fact]
        public async Task CountAsync_Should_Return_Zero_When_No_Records()
        {
            // Arrange
            var repository = CreateRepository();
            // Act
            var result = await repository.CountAsync();
            // Assert
            Assert.Equal(ResultCodeSuccess, result.Code);
            Assert.Equal(ResultCodeSuccess, result.Value);
        }

        [Fact]
        public async Task UpdateAsync_Should_Handle_NonExistent_Item()
        {
            // Arrange
            var repository = CreateRepository();
            var notExistsItem = TestDataGenerator.CreateFake<TopicWorkflowDto>("tr");
            // Act
            var result = await repository.UpdateAsync(notExistsItem);
            // Assert
            // Repository should return 404 for non-existent entity
            Assert.Equal(ResultCodeNotFound, result.Code);
        }

        [Fact]
        public async Task DeleteAsync_Should_Handle_NonExistent_Item()
        {
            // Arrange
            var repository = CreateRepository();
            var notExistsItem = TestDataGenerator.CreateFake<TopicWorkflowDto>("tr");
            // Act
            var result = await repository.DeleteAsync(notExistsItem);
            // Assert
            // Repository should return 404 for non-existent entity
            Assert.Equal(ResultCodeNotFound, result.Code);
        }

        [Fact]
        public async Task ListAsync_Should_Order_By_Id_Ascending()
        {
            // Arrange
            var repository = CreateRepository();
            var fakeItems = TestDataGenerator.CreateFakes<TopicWorkflowDto>();
            foreach (var fakeItem in fakeItems)
            {
                var insertResponse = await repository.InsertAsync(fakeItem);
                if (insertResponse.Code != ResultCodeSuccess)
                {
                    Console.WriteLine(insertResponse.Message);
                }

                Assert.Equal(ResultCodeSuccess, insertResponse.Code);
            }

            // Act
            var result = await repository.ListAsync();
            // Assert
            Assert.Equal(ResultCodeSuccess, result.Code);
            Assert.Equal(3, result.Value.Count);
            // Should be ordered by Id ascending
            for (var i = 0; i < result.Value.Count - 1; i++)
            {
                Assert.True(result.Value[i].Id <= result.Value[i + 1].Id);
            }
        }

        public void Dispose()
        {
        // Cleanup is handled by using statements
        }
    }
}