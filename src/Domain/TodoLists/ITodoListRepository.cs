using Domain.Common;

namespace Domain.TodoLists;

public interface ITodoListRepository
{
    Task<List<TodoList>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<TodoList?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(TodoList todoList, CancellationToken cancellationToken = default);
    Task UpdateAsync(TodoList todoList, CancellationToken cancellationToken = default);
    Task DeleteAsync(TodoList todoList, CancellationToken cancellationToken = default);
    Task ReorderItems(List<ItemOrder> itemOrders, Guid userId, CancellationToken cancellationToken = default);
}