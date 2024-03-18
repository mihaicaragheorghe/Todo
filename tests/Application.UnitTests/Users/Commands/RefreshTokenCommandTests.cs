using Application.Common.Errors;
using Application.Common.Interfaces;
using Application.Users.Commands;
using Application.Users.Contracts;

using Domain.Users;

using FluentAssertions;

using NSubstitute;

using TestCommon.Users;

namespace Application.UnitTests.Users.Commands;

public class RefreshTokenCommandTests
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IRefreshTokenValidator _refreshTokenValidator;
    private readonly RefreshTokenCommandHandler _sut;

    public RefreshTokenCommandTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _tokenGenerator = Substitute.For<ITokenGenerator>();
        _refreshTokenValidator = Substitute.For<IRefreshTokenValidator>();
        _sut = new RefreshTokenCommandHandler(_userRepository, _tokenGenerator, _refreshTokenValidator);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenRefreshTokenIsValid()
    {
        // Arrange
        var command = RefreshTokenCommandFactory.Create();
        var user = UserFactory.CreateUser(id: command.UserId);
        var accessToken = "accessToken";
        var refreshToken = "refreshToken";
        _refreshTokenValidator
            .Validate(command.RefreshToken)
            .Returns(true);
        _refreshTokenValidator
            .GetUserIdFromToken(command.RefreshToken)
            .Returns(user.Id.ToString());
        _userRepository
            .GetByIdAsync(user.Id, Arg.Any<CancellationToken>())
            .Returns(user);
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
    public async Task Handle_ShouldReturnFailure_WhenRefreshTokenIsInvalid()
    {
        // Arrange
        var command = RefreshTokenCommandFactory.Create();
        _refreshTokenValidator
            .Validate(command.RefreshToken)
            .Returns(false);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(UserErrors.RefreshTokenValidationFailed);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenUserIdFromTokenIsDifferent()
    {
        // Arrange
        var command = RefreshTokenCommandFactory.Create();
        _refreshTokenValidator
            .Validate(command.RefreshToken)
            .Returns(true);
        _refreshTokenValidator
            .GetUserIdFromToken(command.RefreshToken)
            .Returns(Guid.NewGuid().ToString());
        _userRepository
            .GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(UserFactory.CreateUser());

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(UserErrors.RefreshTokenValidationFailed);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenUserIsNotFound()
    {
        // Arrange
        var command = RefreshTokenCommandFactory.Create();
        _refreshTokenValidator
            .Validate(command.RefreshToken)
            .Returns(true);
        _refreshTokenValidator
            .GetUserIdFromToken(command.RefreshToken)
            .Returns(command.UserId.ToString());
        _userRepository
            .GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((User?)null);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(UserErrors.RefreshTokenValidationFailed);
    }
}