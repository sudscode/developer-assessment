using System;
using System.ComponentModel.DataAnnotations;

namespace TodoList.Domain
{
    public class TodoItem
    {
        public Guid Id { get; set; }
        
        public string Description { get; set; } = null!;
                
        public bool IsCompleted { get; set; }
    }
}
