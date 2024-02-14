using Application.Core;

using Domain.TodoLists;

using MediatR;

namespace Application.TodoLists.Commands.CreateTodoList
{
    public record CreateTodoListCommand(
        Guid UserId,
        string Title
    ) : IRequest<Result<TodoList>>;
}