using Application.Core;

using MediatR;

namespace Application.TodoLists.Commands.ToggleTodoListArchieve;

public record ToggleTodoListArchieveCommand(Guid TodoListId, Guid UserId, bool IsArchived)
    : IRequest<Result<Unit>>;