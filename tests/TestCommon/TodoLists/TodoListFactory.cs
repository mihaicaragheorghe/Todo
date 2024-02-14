using Domain.TodoLists;

using TestCommon.Constants;

namespace TestCommon.TodoLists;

public static class TodoListFactory
{
    public static TodoList CreateTodoList(
        Guid? userId = null,
        string title = "New List",
        Guid? id = null)
    {
        return new(
            userId ?? TestConstants.Users.Id,
            title,
            id ?? TestConstants.TodoLists.Id);
    }

    public static List<TodoList> CreateTodoLists(int count, Guid? userId = null) =>
        Enumerable
            .Range(1, count)
            .Select(i => CreateTodoList(userId, TestConstants.TodoLists.Title + i, Guid.NewGuid()))
            .ToList();
}