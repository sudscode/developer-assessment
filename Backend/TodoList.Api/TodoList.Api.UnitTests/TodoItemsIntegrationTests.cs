using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TodoList.Api.Responses;
using Xunit;
using FluentAssertions;
using System.Net.Http;
using TodoList.Api.Requests;
using System.Linq;
using System;
using Microsoft.AspNetCore.Http;

namespace TodoList.Api.UnitTests
{

    public class TodoItemsIntegrationTests : IClassFixture<TestWebApplicationFactory<Program>>
    {
        private readonly TestWebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public TodoItemsIntegrationTests(TestWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }


        [Fact]
        public async Task GetTodoItems_Should_ReturnStatusCode200OK()
        {
            // Act
            var expectedItemsCount = TestHelper.SeedData().Count;
            var response = await _client.GetAsync("/api/TodoItems");
            var todoResponse = response.Content.ReadFromJsonAsync<List<TodoItemResponse>>();

            // Assert
            response.EnsureSuccessStatusCode();

            todoResponse.Result.Count.Should().Be(expectedItemsCount);
        }

        [Fact]
        public async Task PostTodoItem_Should_ReturnStatusCode200OK()
        {
            //Arrange
            var expectedItemsCount = TestHelper.SeedData().Count + 1;
            var request = new TodoItemRequest
            {
                Id = Guid.NewGuid(),
                Description = "New Description",
                IsCompleted = false
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/TodoItems", request);

            // Assert
            response.EnsureSuccessStatusCode();

            var getResponse = await _client.GetAsync("/api/TodoItems");
            var todoResponse = getResponse.Content.ReadFromJsonAsync<List<TodoItemResponse>>();

            todoResponse.Result.Count.Should().Be(expectedItemsCount);
            var newTodoItem = todoResponse.Result.FirstOrDefault(x => x.Id == request.Id);

            newTodoItem.Should().NotBeNull();
            newTodoItem.Id.Should().Be(request.Id);
            newTodoItem.Description.Should().Be(request.Description);
            newTodoItem.IsCompleted.Should().Be(request.IsCompleted);
        }

        [Theory]
        [MemberData(nameof(GetConflictRequests))]
        public async Task PostTodoItem_Should_ReturnStatusCode409Conflict(TodoItemRequest request)
        {
            //Arrange
            var expectedItemsCount = TestHelper.SeedData().Count;

            // Act
            var response = await _client.PostAsJsonAsync("/api/TodoItems", request);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Conflict);

            var getResponse = await _client.GetAsync("/api/TodoItems");
            var todoResponse = getResponse.Content.ReadFromJsonAsync<List<TodoItemResponse>>();

            todoResponse.Result.Count.Should().Be(expectedItemsCount);
         }

        public static IEnumerable<object[]> GetConflictRequests()
        {
            yield return new object[]
            {
                new TodoItemRequest
                {
                    Id = Guid.NewGuid(),
                    Description = TestHelper.SeedData()[0].Description,
                    IsCompleted = false
                }
            };

            yield return new object[]
            {
                new TodoItemRequest
                {
                    Id = TestHelper.SeedData()[0].Id,
                    Description = "Brand new Description",
                    IsCompleted = false
                }
            };
        }
    }
}
