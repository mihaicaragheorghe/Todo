using Domain.Users;

namespace TestCommon.Users;

public static class UserFactory
{
    public static User CreateUser(
        Guid? id = null,
        string email = "kendallroy@waystar.com",
        string name = "Kendall Roy",
        string profilePictureUrl = "profilePictureUrl",
        DateTime? createdAtUtc = null,
        List<string>? roles = null) =>
            new(id: id ?? Guid.NewGuid(),
                email: email,
                name: name,
                profilePictureUrl: profilePictureUrl,
                createdAtUtc: createdAtUtc ?? DateTime.UtcNow,
                roles: roles ?? []);
}