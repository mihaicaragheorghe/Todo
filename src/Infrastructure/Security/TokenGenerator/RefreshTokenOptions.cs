namespace Infrastructure.Security.TokenGenerator;

public class RefreshTokenOptions
{
    public string Secret { get; set; } = null!;
    public int ExpiresInHours { get; set; }
}