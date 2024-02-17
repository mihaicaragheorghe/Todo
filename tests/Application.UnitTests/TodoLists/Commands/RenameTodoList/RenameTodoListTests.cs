using Application.Common.Errors;
using Application.TodoLists.Commands.RenameTodoList;

using Domain.Common;
using Domain.TodoLists;

using FluentAssertions;

using NSubstitute;

using TestCommon.TodoLists;

namespace Application.UnitTests.TodoLists.Commands.RenameTodoList;

public class RenameTodoListTests
{
    private readonly ITodoListRepository _todoListRepository;
    private readonly RenameTodoListCommandHandler _handler;

    public RenameTodoListTests()
    {
        _todoListRepository = Substitute.For<ITodoListRepository>();
        _handler = new RenameTodoListCommandHandler(_todoListRepository);
    }

    [Fact]
    public async Task Handle_ShouldUpdateTodoListTitle_WhenValidCommand()
    {
        // Arrange
        var todoList = TodoListFactory.CreateTodoList();
        var command = new RenameTodoListCommand(
            Id: todoList.Id,
            UserId: todoList.UserId,
            Name: "Changed title");
        _todoListRepository
            .GetByIdAsync(command.Id, default)
            .Returns(todoList);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsSuccessful.Should().BeTrue();
        todoList.Name.Should().Be(command.Name);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenListNotFound()
    {
        // Arrange
        var command = new RenameTodoListCommand(
            Id: Guid.NewGuid(),
            UserId: Guid.NewGuid(),
            Name: "Changed title");
        _todoListRepository
            .GetByIdAsync(command.Id, default)
            .Returns((TodoList?)null);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TodoListErrors.NotFound);
    }

    [Fact]
    public async Task Handle_ShouldThrowDomainException_WhenNameIsEmpty()
    {
        // Arrange
        var todoList = TodoListFactory.CreateTodoList();
        var command = new RenameTodoListCommand(
            Id: todoList.Id,
            UserId: todoList.UserId,
            Name: string.Empty);
        _todoListRepository
            .GetByIdAsync(command.Id, default)
            .Returns(todoList);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, default);

        // Assert
        await act.Should().ThrowAsync<DomainException>();
    }

    [Fact]
    public async Task Handle_ShouldThrowDomainException_WhenNameIsTooLong()
    {
        // Arrange
        var todoList = TodoListFactory.CreateTodoList();
        var command = new RenameTodoListCommand(
            Id: todoList.Id,
            UserId: todoList.UserId,
            Name: new string('A', TodoListConstants.NameMaxLength + 1));
        _todoListRepository
            .GetByIdAsync(command.Id, default)
            .Returns(todoList);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, default);

        // Assert
        await act.Should().ThrowAsync<DomainException>();
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenUserIdDoesNotMatch()
    {
        // Arrange
        var todoList = TodoListFactory.CreateTodoList();
        var command = new RenameTodoListCommand(
            Id: todoList.Id,
            UserId: Guid.NewGuid(),
            Name: "Changed title");
        _todoListRepository
            .GetByIdAsync(command.Id, default)
            .Returns(todoList);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TodoListErrors.NotFound);
    }
}