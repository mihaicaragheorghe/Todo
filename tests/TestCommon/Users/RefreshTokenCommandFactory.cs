using Application.Users.Commands;

namespace TestCommon.Users;

public static class RefreshTokenCommandFactory
{
    public static RefreshTokenCommand Create(
        Guid? userId = null,
        string refreshToken = "refreshToken",
        string accessToken = "accessToken") =>
            new(userId ?? Guid.NewGuid(), accessToken, refreshToken);
}