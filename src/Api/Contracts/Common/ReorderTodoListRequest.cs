using Domain.Common;

namespace Api.Contracts.Common;

public record ReorderTodoListRequest(List<ItemOrder> Items);