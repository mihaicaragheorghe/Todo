using Application.Users.Commands;

using Domain.Users;

using FluentAssertions;

namespace Application.UnitTests.Users;

public class RefreshTokenCommandValidatorTests
{
    private readonly RefreshTokenCommandValidator _validator;
    private readonly RefreshTokenCommand _command;

    public RefreshTokenCommandValidatorTests()
    {
        _validator = new RefreshTokenCommandValidator();
        _command = new RefreshTokenCommand(
            UserId: Guid.NewGuid(),
            AccessToken: "token",
            RefreshToken: "refreshToken");
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
    public void IsValid_ShouldBeFalse_WhenUserIdIsEmpty()
    {
        // Arrange
        var command = _command with { UserId = Guid.Empty };
        
        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void IsValid_ShouldBeFalse_WhenAccessTokenIsEmpty()
    {
        // Arrange
        var command = _command with { AccessToken = string.Empty };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void IsValid_ShouldBeFalse_WhenRefreshTokenIsEmpty()
    {
        // Arrange
        var command = _command with { RefreshToken = string.Empty };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}