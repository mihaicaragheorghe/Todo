namespace Infrastructure.Security.CurrentUser;

public interface ICurrentUserProvider
{
    CurrentUser? GetCurrentUser();
}