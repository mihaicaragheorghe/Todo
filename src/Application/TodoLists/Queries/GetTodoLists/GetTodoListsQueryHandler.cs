using Application.Core;

using Domain.TodoLists;

using MediatR;

namespace Application.TodoLists.Queries.GetTodoLists;

public class GetTodoListsQueryHandler(ITodoListRepository todoListRepository)
    : IRequestHandler<GetTodoListsQuery, Result<List<TodoList>>>
{
    public async Task<Result<List<TodoList>>> Handle(GetTodoListsQuery query, CancellationToken cancellationToken)
    {
        return await todoListRepository.GetByUserIdAsync(query.UserId, cancellationToken);
    }
}