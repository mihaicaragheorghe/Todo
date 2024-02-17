using Application.TodoLists.Commands.RenameTodoList;

using Domain.TodoLists;

using FluentAssertions;

namespace Application.UnitTests.TodoLists.Commands.RenameTodoList;

public class RenameTodoListValidationTests
{
    private readonly RenameTodoListCommandValidator _validator = new();

    [Fact]
    public async Task ValidateAsync_ShouldReturnValid_WhenCommandIsValid()
    {
        // Arrange
        var command = new RenameTodoListCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Changed title");

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task ValidateAsync_ShouldReturnError_WhenNameIsEmpty()
    {
        // Arrange
        var command = new RenameTodoListCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            string.Empty);

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(TodoList.Name));
    }

    [Fact]
    public async Task ValidateAsync_ShouldReturnError_WhenNameIsTooLong()
    {
        // Arrange
        var command = new RenameTodoListCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            new string('A', TodoListConstants.NameMaxLength + 1));

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(TodoList.Name));
    }

    [Fact]
    public async Task ValidateAsync_ShouldReturnError_WhenIdIsEmpty()
    {
        // Arrange
        var command = new RenameTodoListCommand(
            Guid.Empty,
            Guid.NewGuid(),
            "Changed title");

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(RenameTodoListCommand.Id));
    }
}