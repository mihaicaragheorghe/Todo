using Application.Core;

using MediatR;

namespace Application.TodoLists.Commands.RenameTodoList;

public record RenameTodoListCommand(Guid Id, string Name)
    : IRequest<Result<Unit>>;