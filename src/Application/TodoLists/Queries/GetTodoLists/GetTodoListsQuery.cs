using Application.Core;

using Domain.TodoLists;

using MediatR;

namespace Application.TodoLists.Queries.GetTodoLists;

public record GetTodoListsQuery(Guid UserId)
    : IRequest<Result<List<TodoList>>>;