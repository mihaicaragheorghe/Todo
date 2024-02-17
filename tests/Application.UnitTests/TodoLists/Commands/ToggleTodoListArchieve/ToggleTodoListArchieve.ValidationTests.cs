using Application.TodoLists.Commands.ToggleTodoListArchieve;

using FluentAssertions;

using TestCommon.Constants;

namespace Application.UnitTests.TodoLists.Commands.ToggleTodoListArchieve;

public class ToggleTodoListArchieveValidationTests
{
    private readonly ToggleTodoListArchieveCommandValidator _validator = new();

    [Fact]
    public async Task ValidateAsync_ShouldBeValid()
    {
        // Arrange
        var command = new ToggleTodoListArchieveCommand(
            TodoListId: Guid.NewGuid(),
            UserId: TestConstants.Users.Id,
            IsArchived: true);

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task ValidateAsync_ShouldHaveError_WhenTodoListIdIsEmpty()
    {
        // Arrange
        var command = new ToggleTodoListArchieveCommand(
            TodoListId: Guid.Empty,
            UserId: TestConstants.Users.Id,
            IsArchived: true);

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors
            .Should()
            .ContainSingle(e => e.PropertyName == nameof(ToggleTodoListArchieveCommand.TodoListId));
    }

    [Fact]
    public async Task ValidateAsync_ShouldHaveError_WhenUserIdIsEmpty()
    {
        // Arrange
        var command = new ToggleTodoListArchieveCommand(
            TodoListId: Guid.NewGuid(),
            UserId: Guid.Empty,
            IsArchived: true);

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors
            .Should()
            .ContainSingle(e => e.PropertyName == nameof(ToggleTodoListArchieveCommand.UserId));
    }
}