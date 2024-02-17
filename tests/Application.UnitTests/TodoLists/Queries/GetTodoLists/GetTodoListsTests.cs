using Application.TodoLists.Queries.GetTodoLists;

using Domain.TodoLists;

using FluentAssertions;

using NSubstitute;

using TestCommon.Constants;
using TestCommon.TodoLists;

namespace Application.UnitTests.TodoLists.Queries.GetTodoLists;

public class GetTodoLists
{
    private readonly ITodoListRepository _todoListRepository;
    private readonly GetTodoListsQueryHandler _handler;

    public GetTodoLists()
    {
        _todoListRepository = Substitute.For<ITodoListRepository>();
        _handler = new GetTodoListsQueryHandler(_todoListRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnTodoLists_WhenValidQuery()
    {
        // Arrange
        var userId = TestConstants.Users.Id;
        var query = new GetTodoListsQuery(userId);
        var todoLists = TodoListFactory.CreateTodoLists(
            count: 3,
            userId: userId);

        _todoListRepository.GetByUserIdAsync(userId, default)
            .Returns(todoLists);

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        result.IsSuccessful.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(todoLists);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoLists()
    {
        // Arrange
        var query = new GetTodoListsQuery(Guid.NewGuid());

        _todoListRepository.GetByUserIdAsync(query.UserId, default)
            .Returns([]);

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        result.IsSuccessful.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }
}