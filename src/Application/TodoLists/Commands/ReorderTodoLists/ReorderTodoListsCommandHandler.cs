using Application.Core;

using Domain.TodoLists;

using MediatR;

namespace Application.TodoLists.Commands.ReorderTodoLists;

public class ReorderTodoListsCommandHandler(ITodoListRepository todoListRepository)
    : IRequestHandler<ReorderTodoListsCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(ReorderTodoListsCommand command, CancellationToken cancellationToken)
    {
        await todoListRepository.ReorderItems(command.ItemOrders, command.UserId, cancellationToken);

        return Unit.Value;
    }
}