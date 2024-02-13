using Domain.Common;

namespace Domain.Todos;

public class Todo : Entity
{
    public Guid UserId { get; }

    public Guid ListId { get; private set; }

    public DateTime CreatedAtUtc { get; }

    public string Title { get; private set; } = null!;

    public string? Description { get; private set; }

    public DateOnly? DueDateUtc { get; private set; }

    public TimeOnly? DueTimeUtc { get; private set; } 

    public bool IsCompleted { get; private set; }

    public Todo(
        Guid userId,
        Guid listId,
        string title,
        string? description,
        DateOnly? dueDate,
        TimeOnly? dueTime,
        Guid? id = null) : base(id ?? Guid.NewGuid())
    {
        UserId = userId;
        ListId = listId;
        Title = title;
        Description = description;
        DueDateUtc = dueDate;
        DueTimeUtc = dueTime;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public void Complete()
    {
        if (IsCompleted)
        {
            throw new DomainException("Todo is already completed.");
        }

        IsCompleted = true;
    }

    public void UpdateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new DomainException("Title cannot be empty.");
        }

        Title = title;
    }

    public void UpdateDescription(string? description)
    {
        Description = string.IsNullOrWhiteSpace(description) 
            ? null 
            : description;
    }

    public void SetDueDateAndTime(DateOnly? dueDate, TimeOnly? dueTime)
    {
        if (dueTime is not null && dueDate is null)
        {
            throw new DomainException("Due date is required when setting a due time.");
        }

        DueDateUtc = dueDate;
        DueTimeUtc = dueTime;
    }

    private Todo() { }
}