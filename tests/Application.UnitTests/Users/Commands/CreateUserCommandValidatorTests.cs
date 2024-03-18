using Application.Users.Commands;

using Domain.Users;

using FluentAssertions;

namespace Application.UnitTests.Users.Commands;

public class CreateUserCommandValidatorTests
{
    private readonly CreateUserCommandValidator _validator;
    private readonly CreateUserCommand _command;

    public CreateUserCommandValidatorTests()
    {
        _validator = new CreateUserCommandValidator();
        _command = new CreateUserCommand(
            Email: "kendallroy@waystar.com",
            Name: "Kendall Roy",
            Password: "P@ssw0rd");
    }

    [Fact]
    public void IsValid_ShouldBeTrue_WhenCommandIsValid()
    {
        // Act
        var result = _validator.Validate(_command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void IsValid_ShouldBeFalse_WhenEmailIsInvalid()
    {
        // Arrange
        var command = _command with { Email = "kendallroy" };
        
        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void IsValid_ShouldBeFalse_WhenEmailIsEmpty()
    {
        // Arrange
        var command = _command with { Email = string.Empty };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void IsValid_ShouldBeFalse_WhenNameIsEmpty()
    {
        // Arrange
        var command = _command with { Name = string.Empty };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void IsValid_ShouldBeFalse_WhenNameLengthIsGreaterThanMaxLength()
    {
        // Arrange
        var command = _command with { Name = new string('A', UserConstants.NameMaxLength + 1) };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void IsValid_ShouldBeFalse_WhenPasswordIsEmpty()
    {
        // Arrange
        var command = _command with { Password = string.Empty };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void IsValid_ShouldBeFalse_WhenPasswordDoesNotContainNumbers()
    {
        // Arrange
        var command = _command with { Password = "Password" };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void IsValid_ShouldBeFalse_WhenPasswordDoesNotContainUppercaseLetters()
    {
        // Arrange
        var command = _command with { Password = "password1" };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void IsValid_ShouldBeFalse_WhenPasswordDoesNotContainLowercaseLetters()
    {
        // Arrange
        var command = _command with { Password = "PASSWORD1" };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}