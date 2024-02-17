using Application.Todos.Contracts;

namespace Application.TodoLists.Contracts;

public record TodoListDto(
    Guid Id,
    Guid UserId,
    string Name,
    int Order,
    bool IsArchived,
    DateTime CreatedAtUtc,
    List<TodoDto> Todos);