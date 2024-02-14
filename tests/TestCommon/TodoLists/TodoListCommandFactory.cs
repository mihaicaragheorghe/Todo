using Application.TodoLists.Commands.CreateTodoList;

namespace TestCommon.TodoLists;

public static class TodoListCommandFactory
{
    public static CreateTodoListCommand CreateCreateTodoListCommand(Guid? userId = null, string title = "New List") =>
        new(userId ?? Guid.NewGuid(), title);
}