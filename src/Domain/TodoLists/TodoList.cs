using Domain.Common;
using Domain.Todos;

namespace Domain.TodoLists;

public class TodoList : Entity
{
    public Guid UserId { get; }
    
    public string Title { get; private set; } = null!;

    public bool IsArchived { get; private set; }

    public DateTime CreatedAtUtc { get; }

    private readonly List<Todo> _todos = [];

    public IReadOnlyList<Todo> Todos => _todos.AsReadOnly();

    public TodoList(Guid userId, string title, Guid? id = null) 
        : base(id ?? Guid.NewGuid())
    {
        UserId = userId;
        Title = title;
    }

    public void UpdateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new DomainException("Title cannot be empty.");
        }

        Title = title;
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
        var todo = _todos.FirstOrDefault(t => t.Id == todoId) 
            ?? throw new DomainException("Todo not found in the list");

        _todos.Remove(todo);
    }

    public void Archive()
    {
        if (IsArchived)
        {
            throw new DomainException("List is already archived.");
        }

        IsArchived = true;
    }

    public void Unarchive()
    {
        if (!IsArchived)
        {
            throw new DomainException("List is not archived.");
        }

        IsArchived = false;
    }

    private TodoList() { }
}