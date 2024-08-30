using System.ComponentModel.DataAnnotations;
using System;

namespace TodoList.Api.Requests
{
    public class TodoItemRequest
    {
        [Required]
        public Guid Id { get; set; }

        [MinLength(1, ErrorMessage = "Description is required")]

        public string Description { get; set; } = null!;

        [Required]
        public bool IsCompleted { get; set; }
    }
}
