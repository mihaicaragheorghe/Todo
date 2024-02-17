using Application.TodoLists.Commands.ReorderTodoLists;

using Domain.Common;

using FluentAssertions;

namespace Application.UnitTests.TodoLists.Commands.ReorderTodoLists;

public class ReorderTodoListsValidationTests
{
    private readonly ReorderTodoListsCommandValidator _validator = new();

    [Fact]
    public async Task ShouldBeValid()
    {
        // Arrange
        var command = new ReorderTodoListsCommand(
        [
            new(Guid.NewGuid(), 1),
            new(Guid.NewGuid(), 2)
        ]);

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task ShouldHaveError_WhenItemOrdersIsNull()
    {
        // Arrange
        var command = new ReorderTodoListsCommand(null!);

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors
            .Should()
            .ContainSingle(e => e.PropertyName == nameof(ReorderTodoListsCommand.ItemOrders));
    }
}