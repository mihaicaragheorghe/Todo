using Application.Users.Commands;

namespace TestCommon.Users;

public static class CreateUserCommandFactory
{
    public static CreateUserCommand Create(
        string email = "kendallroy@waystar.com",
        string password = "Pa$$w0rd!",
        string name = "Kendall Roy") =>
        new(email, password, name);
}