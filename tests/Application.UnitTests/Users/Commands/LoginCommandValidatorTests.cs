using Application.Users.Commands;

using FluentAssertions;

namespace Application.UnitTests.Users.Commands;

public class LoginCommandValidatorTests
{
    private readonly LoginCommandValidator _validator;
    private readonly LoginCommand _command;

    public LoginCommandValidatorTests()
    {
        _validator = new LoginCommandValidator();
        _command = new LoginCommand(
            Email: "kendallroy@waystar.com",
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
    public void IsValid_ShouldBeFalse_WhenPasswordIsEmpty()
    {
        // Arrange
        var command = _command with { Password = string.Empty };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}