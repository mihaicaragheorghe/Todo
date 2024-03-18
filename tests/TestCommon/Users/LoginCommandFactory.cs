using Application.Users.Commands;

namespace TestCommon.Users;

public static class LoginCommandFactory
{
    public static LoginCommand Create(
        string email = "kendallroy@waystar.com",
        string password = "Pa$$w0rd") =>
            new(email, password);
}