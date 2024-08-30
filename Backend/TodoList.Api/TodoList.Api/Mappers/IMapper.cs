using System;
using System.Collections.Generic;
using TodoList.Api.Requests;
using TodoList.Api.Responses;
using TodoList.Domain;

namespace TodoList.Api.Mappers
{

    public interface IMapper
    {
        TodoItem MapRequestToDomain(TodoItemRequest request);
        TodoItemResponse MapDomainToResponse(TodoItem todoItem);
        List<TodoItemResponse> MapDomainToResponse(List<TodoItem> todoItems);
        TodoItem MapUpdateRequestToDomain(TodoItemUpdateRequest request, Guid id);

    }
}
