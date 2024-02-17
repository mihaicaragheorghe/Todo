using Application.TodoLists.Commands.ReorderTodoLists;

using Domain.Common;
using Domain.TodoLists;

using NSubstitute;

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
    public async Task Handle_ShouldCallReorderItemsOnce()
    {
        // Arrange
        var command = new ReorderTodoListsCommand([
            new ItemOrder(Guid.NewGuid(), 1),
            new ItemOrder(Guid.NewGuid(), 2)
        ]);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _repository.Received(1)
            .ReorderItems(command.ItemOrders, Arg.Any<CancellationToken>());
    }
}