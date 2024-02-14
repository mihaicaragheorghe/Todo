using Application.Common.Errors;
using Application.TodoLists.Queries.GetTodoList;

using Domain.TodoLists;

using FluentAssertions;

using NSubstitute;

using TestCommon.Constants;
using TestCommon.TodoLists;

namespace Application.UnitTests.TodoLists.Queries.GetTodoList;

public class GetTodoListTests
{
    private readonly ITodoListRepository _todoListRepository;
    private readonly GetTodoListQueryHandler _handler;

    public GetTodoListTests()
    {
        _todoListRepository = Substitute.For<ITodoListRepository>();
        _handler = new GetTodoListQueryHandler(_todoListRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnTodoList()
    {
        // Arrange
        var todoList = TodoListFactory.CreateTodoList();
        var query = new GetTodoListQuery(todoList.UserId, todoList.Id);

        _todoListRepository.GetByIdAsync(todoList.Id, default)
            .Returns(todoList);

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        result.IsSuccessful.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(todoList);
    }

    [Fact]
    public async Task Handle_GivenInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var query = new GetTodoListQuery(TestConstants.Users.Id, Guid.NewGuid());

        _todoListRepository.GetByIdAsync(query.Id, default)
            .Returns((TodoList?)null);

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        result.IsSuccessful.Should().BeFalse();
        result.Error.Should().Be(TodoListErrors.NotFound);
    }

    [Fact]
    public async Task Handle_GivenInvalidUserId_ShouldReturnNotFound()
    {
        // Arrange
        var todoList = TodoListFactory.CreateTodoList();
        var query = new GetTodoListQuery(Guid.NewGuid(), todoList.Id);

        _todoListRepository.GetByIdAsync(todoList.Id, default)
            .Returns(todoList);

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        result.IsSuccessful.Should().BeFalse();
        result.Error.Should().Be(TodoListErrors.NotFound);
    }
}