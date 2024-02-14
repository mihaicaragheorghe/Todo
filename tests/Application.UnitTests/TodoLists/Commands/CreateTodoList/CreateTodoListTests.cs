using Application.TodoLists.Commands.CreateTodoList;

using Domain.TodoLists;

using FluentAssertions;

using TestCommon.TodoLists;

namespace Application.UnitTests.TodoLists.Commands.CreateTodoList;

public class CreateTodoListTests
{
    private readonly ITodoListRepository _todoListRepository;
    private readonly CreateTodoListCommandHandler _commandHandler;

    public CreateTodoListTests()
    {
        _todoListRepository = NSubstitute.Substitute.For<ITodoListRepository>();
        _commandHandler = new CreateTodoListCommandHandler(_todoListRepository);
    }

    [Fact]
    public async Task Handle_ShouldPersistTodoList()
    {
        // Arrange
        var command = TodoListCommandFactory.CreateCreateTodoListCommand();

        // Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccessful.Should().BeTrue();
        result.Value?.ShouldBeCreatedFrom(command);
    }
}