using Application.Core;

using MediatR;

namespace Application.TodoLists.Commands.DeleteTodoList;

public record DeleteTodoListCommand(Guid Id, Guid UserId)
    : IRequest<Result<Unit>>;