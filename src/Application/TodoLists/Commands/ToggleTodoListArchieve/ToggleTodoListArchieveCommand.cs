using Application.Core;

using MediatR;

namespace Application.TodoLists.Commands.ArchieveUnarchieve;

public record ToggleTodoListArchieveCommand(Guid TodoListId, bool IsArchived)
    : IRequest<Result<Unit>>;