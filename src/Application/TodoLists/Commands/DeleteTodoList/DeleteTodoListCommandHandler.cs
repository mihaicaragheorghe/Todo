using Application.Common.Errors;
using Application.Core;

using Domain.TodoLists;

using MediatR;

namespace Application.TodoLists.Commands.DeleteTodoList;

public class DeleteTodoListCommandHandler(ITodoListRepository todoListRepository)
    : IRequestHandler<DeleteTodoListCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(DeleteTodoListCommand request, CancellationToken cancellationToken)
    {
        var todoList = await todoListRepository.GetByIdAsync(request.Id, cancellationToken);

        if (todoList?.UserId != request.UserId)
        {
            return TodoListErrors.NotFound;
        }

        await todoListRepository.DeleteAsync(todoList, cancellationToken);

        return Unit.Value;
    }
}