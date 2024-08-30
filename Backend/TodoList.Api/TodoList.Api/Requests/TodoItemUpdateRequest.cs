using System.ComponentModel.DataAnnotations;

namespace TodoList.Api.Requests
{
    public class TodoItemUpdateRequest
    {

        [MinLength(1, ErrorMessage = "Description is required")]

        public string Description { get; set; } = null!;

        [Required]
        public bool IsCompleted { get; set; }
    }
}
