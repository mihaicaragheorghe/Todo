using Application.Common.Errors;
using Application.Users.Queries;

using Domain.Users;

using FluentAssertions;

using NSubstitute;

using TestCommon.Users;

namespace Application.UnitTests.Users.Queries;

public class GetUserByIdQueryTests
{
    private readonly IUserRepository _userRepository;
    private readonly GetUserByIdQueryHandler _sut;

    public GetUserByIdQueryTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _sut = new GetUserByIdQueryHandler(_userRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var query = new GetUserByIdQuery(Guid.NewGuid());
        var user = UserFactory.CreateUser(id: query.Id);
        _userRepository
            .GetByIdAsync(user.Id, Arg.Any<CancellationToken>())
            .Returns(user);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccessful.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(user);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var query = new GetUserByIdQuery(Guid.NewGuid());
        _userRepository
            .GetByIdAsync(query.Id, Arg.Any<CancellationToken>())
            .Returns((User?)null);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(UserErrors.NotFound);
    }
}