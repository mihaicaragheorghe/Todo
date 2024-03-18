using Domain.Common;

namespace Domain.Users;

public class User : Entity
{
    public string Email { get; private set; } = null!;

    public string PasswordHash { get; private set; } = null!;

    public string Name { get; private set; } = null!;

    public string ProfilePictureUrl { get; private set; } = null!;

    public List<string> Roles { get; } = [];

    public User(
        Guid id,
        string email,
        string name,
        string profilePictureUrl,
        List<string> roles)
            : base(id)
    {
        Email = email;
        Name = name;
        ProfilePictureUrl = profilePictureUrl;
        Roles = roles;
    }

    public static User Create(string email, string name) =>
        new(Guid.NewGuid(), email, name, string.Empty, []);

    public void Update(string email, string name, string profilePictureUrl)
    {
        Email = email;
        Name = name;
        ProfilePictureUrl = profilePictureUrl;
    }

    public void SetPasswordHash(string passwordHash) =>
        PasswordHash = passwordHash;

    private User()
    {
        // Required by EF Core
    }
}