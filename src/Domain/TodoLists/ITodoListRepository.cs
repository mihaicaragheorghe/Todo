namespace Domain.TodoLists;

public interface ITodoListRepository
{
    Task<IEnumerable<TodoList>> GetAllAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<TodoList> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(TodoList todoList, CancellationToken cancellationToken = default);
    Task UpdateAsync(TodoList todoList, CancellationToken cancellationToken = default);
    Task DeleteAsync(TodoList todoList, CancellationToken cancellationToken = default);
}