using Application.Common.Errors;
using Application.TodoLists.Commands.ToggleTodoListArchieve;

using Domain.TodoLists;

using FluentAssertions;

using NSubstitute;

using TestCommon.TodoLists;

namespace Application.UnitTests.TodoLists.Commands.ToggleTodoListArchieve;

public class ToggleTodoListArchieveTests
{
    private readonly ITodoListRepository _todoListRepository;
    private readonly ToggleTodoListArchieveCommandHandler _handler;

    public ToggleTodoListArchieveTests()
    {
        _todoListRepository = Substitute.For<ITodoListRepository>();
        _handler = new ToggleTodoListArchieveCommandHandler(_todoListRepository);
    }

    [Fact]
    public async Task Handle_ShouldArchieve_WhenRequestIsValid()
    {
        // Arrange
        var todoList = TodoListFactory.CreateTodoList();
        var command = new ToggleTodoListArchieveCommand(
            TodoListId: todoList.Id,
            UserId: todoList.UserId,
            IsArchived: true);
        _todoListRepository.GetByIdAsync(todoList.Id, default)
            .Returns(todoList);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsSuccessful.Should().BeTrue();
        todoList.IsArchived.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ShouldUnarchieve_WhenRequestIsValid()
    {
        // Arrange
        var todoList = TodoListFactory.CreateTodoList();
        var command = new ToggleTodoListArchieveCommand(
            TodoListId: todoList.Id,
            UserId: todoList.UserId,
            IsArchived: false);
        _todoListRepository.GetByIdAsync(todoList.Id, default)
            .Returns(todoList);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsSuccessful.Should().BeTrue();
        todoList.IsArchived.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenNoTodoList()
    {
        // Arrange
        var command = new ToggleTodoListArchieveCommand(
            TodoListId: Guid.NewGuid(),
            UserId: Guid.NewGuid(),
            IsArchived: true);
        _todoListRepository.GetByIdAsync(command.TodoListId, default)
            .Returns((TodoList?)null);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TodoListErrors.NotFound);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenUserIdDoesNotMatch()
    {
        // Arrange
        var todoList = TodoListFactory.CreateTodoList();
        var command = new ToggleTodoListArchieveCommand(
            TodoListId: todoList.Id,
            UserId: Guid.NewGuid(),
            IsArchived: true);
        _todoListRepository.GetByIdAsync(todoList.Id, default)
            .Returns(todoList);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TodoListErrors.NotFound);
    }
}