using Application.TodoLists.Commands.ReorderTodoLists;

using Domain.Common;

using FluentAssertions;

using TestCommon.Constants;

namespace Application.UnitTests.TodoLists.Commands.ReorderTodoLists;

public class ReorderTodoListsValidationTests
{
    private readonly ReorderTodoListsCommandValidator _validator = new();

    [Fact]
    public async Task ValidateAsync_ShouldBeValid()
    {
        // Arrange
        List<ItemOrder> items =
        [
            new(Guid.NewGuid(), 1),
            new(Guid.NewGuid(), 2)
        ];
        var command = new ReorderTodoListsCommand(items, TestConstants.Users.Id);

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task ValidateAsync_ShouldHaveError_WhenItemOrdersIsEmpty()
    {
        // Arrange
        var command = new ReorderTodoListsCommand([], TestConstants.Users.Id);

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors
            .Should()
            .ContainSingle(e => e.PropertyName == nameof(ReorderTodoListsCommand.ItemOrders));
    }

    [Fact]
    public async Task ValidateAsync_ShouldHaveError_WhenUserIdIsEmpty()
    {
        // Arrange
        List<ItemOrder> items =
        [
            new(Guid.NewGuid(), 1),
            new(Guid.NewGuid(), 2)
        ];
        var command = new ReorderTodoListsCommand(items, Guid.Empty);

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors
            .Should()
            .ContainSingle(e => e.PropertyName == nameof(ReorderTodoListsCommand.UserId));
    }
}