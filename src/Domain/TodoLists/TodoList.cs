using Domain.Common;
using Domain.Todos;

namespace Domain.TodoLists;

public class TodoList : Entity
{
    public Guid UserId { get; }

    public string Name { get; private set; } = null!;

    public int Order { get; private set; }

    public bool IsArchived { get; private set; }

    public DateTime CreatedAtUtc { get; }

    private readonly List<Todo> _todos = [];

    public IReadOnlyList<Todo> Todos => _todos.AsReadOnly();

    public TodoList(Guid userId, string title, Guid? id = null)
        : base(id ?? Guid.NewGuid())
    {
        CreatedAtUtc = DateTime.UtcNow;
        UserId = userId;
        Name = title;
    }

    public void Rename(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new DomainException("Title cannot be empty.");
        }
        if (title.Length > TodoListConstants.NameMaxLength)
        {
            throw new DomainException($"Title cannot be longer than {TodoListConstants.NameMaxLength} characters.");
        }

        Name = title;
    }

    public void AddTodo(Todo todo)
    {
        if (todo.UserId != UserId)
        {
            throw new DomainException("Todo does not belong to the list's user.");
        }

        if (todo.ListId != Id)
        {
            throw new DomainException("Todo does not belong to the list.");
        }

        if (_todos.Any(t => t.Id == todo.Id))
        {
            throw new DomainException("Todo already exists in the list.");
        }

        _todos.Add(todo);
    }

    public void RemoveTodo(Guid todoId)
    {
        var todo = _todos.Find(t => t.Id == todoId)
            ?? throw new DomainException("Todo not found in the list");

        _todos.Remove(todo);
    }

    public void ToggleArchived(bool isArchived)
    {
        IsArchived = isArchived;
    }

    public void SetOrder(int order)
    {
        if (order < 0)
        {
            throw new DomainException("Order cannot be negative.");
        }

        Order = order;
    }

    private TodoList() { }
}