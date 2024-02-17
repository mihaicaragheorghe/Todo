using Application.Core;

using MediatR;

namespace Application.TodoLists.Commands.RenameTodoList;

public record RenameTodoListCommand(Guid Id, Guid UserId, string Name)
    : IRequest<Result<Unit>>;