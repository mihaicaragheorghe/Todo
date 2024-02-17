using Application.Core;

using Domain.Common;

using MediatR;

namespace Application.TodoLists.Commands.ReorderTodoLists;

public record ReorderTodoListsCommand(List<ItemOrder> ItemOrders)
    : IRequest<Result<Unit>>;