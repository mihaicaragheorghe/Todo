using Application.Common.Errors;
using Application.TodoLists.Commands.DeleteTodoList;

using Domain.TodoLists;

using FluentAssertions;

using NSubstitute;

using TestCommon.TodoLists;

namespace Application.UnitTests.TodoLists.Commands.DeleteTodoList;

public class DeleteTodoListCommandTests
{
    private readonly ITodoListRepository _repository;
    private readonly DeleteTodoListCommandHandler _commandHandler;

    public DeleteTodoListCommandTests()
    {
        _repository = Substitute.For<ITodoListRepository>();
        _commandHandler = new DeleteTodoListCommandHandler(_repository);
    }

    [Fact]
    public async Task Handle_ShouldDeleteTodoList_WhenTodoListExists()
    {
        // Arrange
        var todoList = TodoListFactory.CreateTodoList();
        var command = new DeleteTodoListCommand(todoList.Id, todoList.UserId);
        _repository
            .GetByIdAsync(todoList.Id, Arg.Any<CancellationToken>())
            .Returns(todoList);

        // Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenTodoListDoesNotExist()
    {
        // Arrange
        var command = new DeleteTodoListCommand(Guid.NewGuid(), Guid.NewGuid());
        _repository
            .GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns((TodoList?)null);

        // Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TodoListErrors.NotFound);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenTodoListDoesNotBelongToUser()
    {
        // Arrange
        var todoList = TodoListFactory.CreateTodoList();
        var command = new DeleteTodoListCommand(todoList.Id, Guid.NewGuid());
        _repository
            .GetByIdAsync(todoList.Id, Arg.Any<CancellationToken>())
            .Returns(todoList);

        // Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TodoListErrors.NotFound);
    }
}