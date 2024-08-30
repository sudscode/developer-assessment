using TodoList.Domain;

namespace TodoList.Application
{
    public interface ITodoListRepository
    {
        //TODO: could use Result pattern
        //Task<Result<TodoItem?>>  GetTodoItemByIdAsync(Guid id,CancellationToken cancel);

        Task<TodoItem?>  GetTodoItemByIdAsync(Guid id,CancellationToken cancel);
        Task<List<TodoItem>> GetTodoItemsAsync();
        Task<int> AddTodoItemAsync(TodoItem item, CancellationToken cancel);
        
        Task<int> UpdateTodoItemAsync(TodoItem item, CancellationToken cancel);
        bool DescriptionAlreadyInUse(string description);
        bool TodoItemIdExists(Guid id);
    }
}
