using Application.TodoLists.Commands.CreateTodoList;

using Domain.TodoLists;

using FluentAssertions;

namespace Application.UnitTests.TodoLists.Commands.CreateTodoList;

public class CreateTodoListValidationTests
{
    private readonly CreateTodoListCommandValidator _validator;

    public CreateTodoListValidationTests()
    {
        _validator = new CreateTodoListCommandValidator();
    }

    [Fact]
    public void IsValid_ShouldBeFalse_WhenTitleIsEmpty()
    {
        // Arrange
        var command = new CreateTodoListCommand(Guid.NewGuid(), "");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void IsValid_ShouldBeFalse_WhenTitleIsOverMaxLength()
    {
        // Arrange
        var command = new CreateTodoListCommand(
            Guid.NewGuid(),
            new string('A', TodoListConstants.NameMaxLength + 1));

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void IsValid_ShouldBeFalse_WhenUserIdIsEmpty()
    {
        // Arrange
        var command = new CreateTodoListCommand(Guid.Empty, "New List");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}