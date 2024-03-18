namespace Application.Users.Contracts;

public record UserDto(
    Guid Id,
    string Email,
    string Name,
    string ProfilePictureUrl);