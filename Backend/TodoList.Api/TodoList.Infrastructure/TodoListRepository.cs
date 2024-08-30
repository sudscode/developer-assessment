using Microsoft.EntityFrameworkCore;
using TodoList.Application;
using TodoList.Domain;

namespace TodoList.Infrastructure
{
    public class TodoListRepository: ITodoListRepository
    {
        private readonly TodoContext _context;

        public TodoListRepository(TodoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<int> AddTodoItemAsync(TodoItem item, CancellationToken cancel)
        {
            _context.TodoItems.Add(item);
            return await _context.SaveChangesAsync();
        }

        public bool DescriptionAlreadyInUse(string description)
        {
            return  _context.TodoItems
                   .Any(x => x.Description.ToLowerInvariant() == description.ToLowerInvariant() && !x.IsCompleted);
        }

        public async Task<List<TodoItem>> GetTodoItemsAsync()
        {
            return await _context
                            .TodoItems
                            .AsNoTracking()
                            .Where(x => !x.IsCompleted)
                            .ToListAsync();
        }

        public async Task<TodoItem?> GetTodoItemByIdAsync(Guid id, CancellationToken cancel)
        {
            return await _context.TodoItems.FindAsync(id,cancel);
        }

        public async Task<int> UpdateTodoItemAsync(TodoItem item, CancellationToken cancel)
        {
            
            _context.Entry(item).State = EntityState.Modified;

            try
            {
                return await _context.SaveChangesAsync(cancel);
            }
            catch (DbUpdateConcurrencyException)
            {
                //TODO : result pattern
                return 0;
            }
            
        }

        public bool TodoItemIdExists(Guid id)
        {
            return _context.TodoItems.Any(x => x.Id == id);
        }

    }
}
