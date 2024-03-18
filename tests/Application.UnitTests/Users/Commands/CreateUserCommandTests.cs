using Application.Common.Errors;

using Domain.Users;

using FluentAssertions;

using Microsoft.AspNetCore.Identity;

using NSubstitute;

using TestCommon.Users;

namespace Application.Users.Commands;

public class CreateUserCommandHandlerTests
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly CreateUserCommandHandler _sut;

    public CreateUserCommandHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _passwordHasher = Substitute.For<IPasswordHasher<User>>();
        _sut = new CreateUserCommandHandler(_userRepository, _passwordHasher);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenCommandIsValid()
    {
        // Arrange
        var command = CreateUserCommandFactory.Create();
        var hashedPassword = "hashedPassword";
        _userRepository
            .FindByEmailAsync(command.Email, Arg.Any<CancellationToken>())
            .Returns((User?)null);
        _passwordHasher
            .HashPassword(Arg.Any<User>(), command.Password)
            .Returns(hashedPassword);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);
    
        // Assert
        result.Value?.ShouldBeCreatedFrom(command, hashedPassword);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenEmailIsInUse()
    {
        // Arrange
        var command = CreateUserCommandFactory.Create();
        _userRepository
            .FindByEmailAsync(command.Email, Arg.Any<CancellationToken>())
            .Returns(UserFactory.CreateUser());

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Error.Should().Be(UserErrors.EmailInUse);
    }
}