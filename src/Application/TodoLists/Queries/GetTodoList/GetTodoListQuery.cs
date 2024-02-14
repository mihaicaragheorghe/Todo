using Application.Core;

using Domain.TodoLists;

using MediatR;

namespace Application.TodoLists.Queries.GetTodoList;

public record GetTodoListQuery(Guid UserId, Guid Id)
    : IRequest<Result<TodoList>>;
