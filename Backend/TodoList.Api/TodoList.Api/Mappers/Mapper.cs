using System;
using System.Collections.Generic;
using TodoList.Api.Requests;
using TodoList.Api.Responses;
using TodoList.Domain;

namespace TodoList.Api.Mappers
{

    //TODO: Automapper 
    public class Mapper :IMapper
    {
        public TodoItem MapRequestToDomain(TodoItemRequest request)
        {
            return new TodoItem
            {
                Id = request.Id,
                Description = request.Description,
                IsCompleted = request.IsCompleted,
            };
        }
        public TodoItemResponse MapDomainToResponse(TodoItem todoItem)
        {
            return new TodoItemResponse
            {
                Id = todoItem.Id,
                Description = todoItem.Description,
                IsCompleted = todoItem.IsCompleted,
            };
        }
        public List<TodoItemResponse> MapDomainToResponse(List<TodoItem> todoItems)
        {
            var responses = new List<TodoItemResponse>();
            foreach (TodoItem todoItem in todoItems)
            {
                responses.Add(MapDomainToResponse(todoItem));
            }
            return responses;
        }

        public TodoItem MapUpdateRequestToDomain(TodoItemUpdateRequest request,Guid id)
        {
            return new TodoItem
            {
                Id=id,
                Description = request.Description,
                IsCompleted = request.IsCompleted,
            };
        }
    }
}
