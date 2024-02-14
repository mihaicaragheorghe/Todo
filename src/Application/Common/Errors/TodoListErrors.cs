using Application.Core;

namespace Application.Common.Errors;

public static class TodoListErrors
{
    public static Error NotFound => Error.NotFound(
        code: "TodoList.NotFound",
        message: "Todo list not found.");
}