namespace Application.Todos.Contracts;

public record TodoDto(
    Guid Id,
    Guid UserId,
    Guid ListId,
    DateTime CreatedAtUtc,
    string Title,
    string? Description,
    DateOnly? DueDateUtc,
    TimeOnly? DueTimeUtc,
    bool IsCompleted);