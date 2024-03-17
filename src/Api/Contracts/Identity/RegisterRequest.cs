using System.ComponentModel.DataAnnotations;

namespace Api.Contracts.Identity;

public class RegisterRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{4,8}$", ErrorMessage = "Password must be complex")]
    public string Password { get; set; } = null!;

    [Required]
    public string Name { get; set; } = null!;
}