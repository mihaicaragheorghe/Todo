using Application.Users.Commands;

using Domain.Users;

using FluentAssertions;

namespace TestCommon.Users;

public static class UserValidationExtensions
{
    public static void ShouldBeCreatedFrom(this User user, CreateUserCommand command, string passwordHash)
    {
        user.Email.Should().Be(command.Email);
        user.Name.Should().Be(command.Name);
        user.PasswordHash.Should().Be(passwordHash);
        user.Id.Should().NotBeEmpty();
        user.CreatedAtUtc.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }
}