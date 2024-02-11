namespace Domain.Todos;

public interface ITodoRepository
{
    Task<IEnumerable<Todo>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default);
    Task<Todo> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Todo todo, CancellationToken cancellationToken = default);
    Task UpdateAsync(Todo todo, CancellationToken cancellationToken = default);
    Task DeleteAsync(Todo todo, CancellationToken cancellationToken = default);
}