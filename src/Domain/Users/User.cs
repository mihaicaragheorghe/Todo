using Domain.Common;

namespace Domain.Users;

public class User : Entity
{
    public string Email { get; private set; }

    public string Name { get; private set; }

    public string ProfilePictureUrl { get; private set; }

    public List<string> Roles { get; }

    public User(Guid id, string email, string name, string profilePictureUrl, List<string> roles)
        : base(id)
    {
        Email = email;
        Name = name;
        ProfilePictureUrl = profilePictureUrl;
        Roles = roles;
    }

    public static User Create(string email, string name, string profilePictureUrl, List<string> roles) =>
        new(Guid.NewGuid(), email, name, profilePictureUrl, roles);

    public void Update(string email, string name, string profilePictureUrl)
    {
        Email = email;
        Name = name;
        ProfilePictureUrl = profilePictureUrl;
    }
}