using Application.TodoLists.Commands.RenameTodoList;

using Domain.TodoLists;

using FluentAssertions;

namespace Application.UnitTests.TodoLists.Commands.RenameTodoList;

public class RenameTodoListValidationTests
{
    private readonly RenameTodoListCommandValidator _validator;

    public RenameTodoListValidationTests()
    {
        _validator = new RenameTodoListCommandValidator();
    }

    [Fact]
    public async Task Validate_WithValidCommand_ShouldReturnTrue()
    {
        // Arrange
        var command = new RenameTodoListCommand(Guid.NewGuid(), "Changed title");

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_WithEmptyName_ShouldReturnFalse()
    {
        // Arrange
        var command = new RenameTodoListCommand(Guid.NewGuid(), string.Empty);

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(TodoList.Name));
    }

    [Fact]
    public async Task Validate_WithTooLongName_ShouldReturnFalse()
    {
        // Arrange
        var command = new RenameTodoListCommand(
            Guid.NewGuid(),
            new string('A', TodoListConstants.NameMaxLength + 1));

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(TodoList.Name));
    }
}