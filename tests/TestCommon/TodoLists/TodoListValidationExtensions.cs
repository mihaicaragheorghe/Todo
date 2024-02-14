using Application.TodoLists.Commands.CreateTodoList;
using Domain.TodoLists;
using FluentAssertions;

namespace TestCommon.TodoLists;

public static class TodoListValidationExtensions
{
    public static void ShouldBeCreatedFrom(this TodoList todoList, CreateTodoListCommand command)
    {
        todoList.UserId.Should().Be(command.UserId);
        todoList.Name.Should().Be(command.Title);
        todoList.IsArchived.Should().BeFalse();
        todoList.Todos.Should().BeEmpty();
        todoList.Id.Should().NotBeEmpty();
        todoList.CreatedAtUtc.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }
}