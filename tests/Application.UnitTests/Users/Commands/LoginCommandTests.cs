using Application.Common.Errors;
using Application.Common.Interfaces;
using Application.Users.Commands;
using Application.Users.Contracts;

using Domain.Users;

using FluentAssertions;

using Microsoft.AspNetCore.Identity;

using NSubstitute;

using TestCommon.Users;

namespace Application.UnitTests.Users.Commands;

public class LoginCommandTests
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly LoginCommandHandler _sut;

    public LoginCommandTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _tokenGenerator = Substitute.For<ITokenGenerator>();
        _passwordHasher = Substitute.For<IPasswordHasher<User>>();
        _sut = new LoginCommandHandler(_userRepository, _tokenGenerator, _passwordHasher);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenCredentialsAreValid()
    {
        // Arrange
        var command = LoginCommandFactory.Create();
        var user = UserFactory.CreateUser();
        var accessToken = "accessToken";
        var refreshToken = "refreshToken";
        _userRepository
            .FindByEmailAsync(command.Email, Arg.Any<CancellationToken>())
            .Returns(user);
        _passwordHasher
            .VerifyHashedPassword(user, user.PasswordHash, command.Password)
            .Returns(PasswordVerificationResult.Success);
        _tokenGenerator
            .GenerateAccessToken(user.Id, user.Email, user.Name, user.Roles)
            .Returns(accessToken);
        _tokenGenerator
            .GenerateRefreshToken(user.Id)
            .Returns(refreshToken);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccessful.Should().BeTrue();
        result.Value?.Should().BeEquivalentTo(new AuthenticationResult(
            user.Id,
            user.Email,
            user.Name,
            accessToken,
            refreshToken));
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenUserIsNotFound()
    {
        // Arrange
        var command = LoginCommandFactory.Create();
        _userRepository
            .FindByEmailAsync(command.Email, Arg.Any<CancellationToken>())
            .Returns((User?)null);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccessful.Should().BeFalse();
        result.Error.Should().Be(UserErrors.InvalidCredentials);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenPasswordIsInvalid()
    {
        // Arrange
        var command = LoginCommandFactory.Create();
        var user = UserFactory.CreateUser();
        _userRepository
            .FindByEmailAsync(command.Email, Arg.Any<CancellationToken>())
            .Returns(user);
        _passwordHasher
            .VerifyHashedPassword(user, user.PasswordHash, command.Password)
            .Returns(PasswordVerificationResult.Failed);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccessful.Should().BeFalse();
        result.Error.Should().Be(UserErrors.InvalidCredentials);
    }
}