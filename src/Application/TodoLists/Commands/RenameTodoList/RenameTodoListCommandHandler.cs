using Application.Common.Errors;
using Application.Core;

using Domain.TodoLists;

using MediatR;

namespace Application.TodoLists.Commands.RenameTodoList;

public class RenameTodoListCommandHandler(ITodoListRepository todoListRepository)
    : IRequestHandler<RenameTodoListCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(RenameTodoListCommand command, CancellationToken cancellationToken)
    {
        var todoList = await todoListRepository.GetByIdAsync(command.Id, cancellationToken);

        if (todoList == null) return TodoListErrors.NotFound;

        todoList.Rename(command.Name);

        await todoListRepository.UpdateAsync(todoList, cancellationToken);

        return Unit.Value;
    }
}