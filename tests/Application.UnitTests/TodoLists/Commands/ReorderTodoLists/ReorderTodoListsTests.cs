using Application.TodoLists.Commands.ReorderTodoLists;

using Domain.Common;
using Domain.TodoLists;

using FluentAssertions;

using NSubstitute;

using TestCommon.Constants;

namespace Application.UnitTests.TodoLists.Commands.ReorderTodoLists;

public class ReorderTodoListsTests
{
    private readonly ITodoListRepository _repository;
    private readonly ReorderTodoListsCommandHandler _handler;

    public ReorderTodoListsTests()
    {
        _repository = Substitute.For<ITodoListRepository>();
        _handler = new ReorderTodoListsCommandHandler(_repository);
    }

    [Fact]
    public async Task Handle_ShouldBeSuccessful_WhenValidCommand()
    {
        // Arrange
        List<ItemOrder> items =
        [
            new ItemOrder(Guid.NewGuid(), 1),
            new ItemOrder(Guid.NewGuid(), 2)
        ];
        var command = new ReorderTodoListsCommand(items, TestConstants.Users.Id);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }
}