using Xunit;
using Moq;
using TodoList.Application;
using Microsoft.Extensions.Logging;
using TodoList.Api.Controllers;
using TodoList.Api.Mappers;
using System.Collections.Generic;
using TodoList.Domain;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using TodoList.Api.Responses;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Threading;

namespace TodoList.Api.UnitTests
{
    public class TodoItemsControllerTests
    {
        private readonly Mock<ITodoListRepository> _repositoryMock;
        private readonly Mock<ILogger<TodoItemsController>> _loggerMock;
        private readonly IMapper _mapper;
        private readonly TodoItemsController _controller;

        public TodoItemsControllerTests()
        {
            _repositoryMock = new Mock<ITodoListRepository>();
            _loggerMock = new Mock<ILogger<TodoItemsController>>();
            _mapper = new Mapper();
            _controller = new TodoItemsController(_loggerMock.Object, _repositoryMock.Object, _mapper);
            
        }


        //Note : not all endpoints covered. just a few for sample
        [Fact]

        public async Task GetTodoItems_Should_ReturnStatusCode200OK()
        {
            //Arrange
            var todoItems = TestHelper.SeedData();
            _repositoryMock.Setup(c => c.GetTodoItemsAsync()).ReturnsAsync(todoItems);

            //Act
            var result = await _controller.GetTodoItems() as ObjectResult;
            var response = result.Value as IEnumerable<TodoItemResponse>;

            //Assert

            result.Value.Should().NotBeNull();
            response.Count().Should().Be(todoItems.Count);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            foreach (var item in todoItems)
            {
                var expectedValue = response.FirstOrDefault(x => x.Id == item.Id);
                expectedValue.Should().NotBeNull();
                expectedValue.Description.Should().Be(item.Description);
                expectedValue.IsCompleted.Should().Be(item.IsCompleted);
            }
        }

        [Fact]
        public async Task GetTodoItem_Should_ReturnStatusCode200OK()
        {
            //Arrange
            var todoItems = TestHelper.SeedData();
            _repositoryMock.Setup(c => c.GetTodoItemByIdAsync(todoItems[0].Id, It.IsAny<CancellationToken>())).ReturnsAsync(todoItems[0]);

            //Act
            var result = await _controller.GetTodoItem(todoItems[0].Id) as ObjectResult;
            
            //Assert
            result.Should().NotBeNull();
            result.Value.Should().BeOfType<TodoItemResponse>();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            var response = result.Value as TodoItemResponse;

            response.Id.Should().Be(todoItems[0].Id);
            response.Description.Should().Be(todoItems[0].Description);
            response.IsCompleted.Should().Be(todoItems[0].IsCompleted);
        }

        [Fact]
        public async Task GetTodoItem_Should_ReturnStatusCode404NotFound()
        {
            //Arrange
            var todoItems = TestHelper.SeedData();
            _repositoryMock.Setup(c => c.GetTodoItemByIdAsync(todoItems[1].Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(default(TodoItem));

            //Act
            var result = await _controller.GetTodoItem(todoItems[1].Id);

            //Assert
            result.Should().NotBeNull();
            ((StatusCodeResult)result).StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }
    }
}
