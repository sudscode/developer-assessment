using System;

namespace TodoList.Api.Responses
{
    public class TodoItemResponse
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = null!;
        public bool IsCompleted { get; set; }
    }
}
