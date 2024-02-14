using System.Security.Claims;

using Microsoft.AspNetCore.Http;

namespace Infrastructure.Security.CurrentUser;

public class CurrentUserProvider(IHttpContextAccessor httpContextAccessor)
    : ICurrentUserProvider
{
    public CurrentUser? GetCurrentUser()
    {
        var userId = GetSingleClaimValue(ClaimTypes.NameIdentifier);
        var email = GetClaimValue(ClaimTypes.Email);
        var name = GetClaimValue(ClaimTypes.Name);
        var roles = GetClaimValues(ClaimTypes.Role);

        if (userId is null || email is null || name is null)
        {
            return null;
        }

        return new CurrentUser(Guid.Parse(userId), email, name, roles);
    }

    private string? GetClaimValue(string claimType) =>
        httpContextAccessor.HttpContext?.User?.FindFirst(claimType)?.Value;

    private List<string> GetClaimValues(string claimType) =>
        httpContextAccessor.HttpContext?.User?.FindAll(claimType)
            .Select(c => c.Value)
            .ToList() ?? [];

    private string GetSingleClaimValue(string claimType) =>
        httpContextAccessor.HttpContext?.User?.Claims
            .Single(c => c.Type == claimType)
            .Value ?? throw new ArgumentNullException(claimType);
}
