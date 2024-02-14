using Application.Common.Errors;
using Application.Core;

using Domain.TodoLists;

using MediatR;

namespace Application.TodoLists.Queries.GetTodoList;

public class GetTodoListQueryHandler(ITodoListRepository todoListRepository)
    : IRequestHandler<GetTodoListQuery, Result<TodoList>>
{
    public async Task<Result<TodoList>> Handle(GetTodoListQuery query, CancellationToken cancellationToken)
    {
        var list = await todoListRepository.GetByIdAsync(query.Id, cancellationToken);

        if (list?.UserId != query.UserId)
        {
            return TodoListErrors.NotFound;
        }

        return list;
    }
}