using System.Security.Claims;

using Application.Common.Errors;
using Application.Common.Interfaces;
using Application.Core;
using Application.Users.Contracts;

using Domain.Users;

using MediatR;

namespace Application.Users.Commands;

public class RefreshTokenCommandHandler(
    IUserRepository userRepository,
    ITokenGenerator tokenGenerator,
    IRefreshTokenValidator refreshTokenValidator)
        : IRequestHandler<RefreshTokenCommand, Result<AuthenticationResult>>
{
    public async Task<Result<AuthenticationResult>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        if (!refreshTokenValidator.Validate(request.RefreshToken))
        {
            return UserErrors.RefreshTokenValidationFailed;
        }

        var userId = refreshTokenValidator.GetUserIdFromToken(request.RefreshToken);

        if (userId != request.UserId.ToString())
        {
            return UserErrors.RefreshTokenValidationFailed;
        }

        var user = await userRepository.GetByIdAsync(Guid.Parse(userId), cancellationToken);

        if (user is null)
        {
            return UserErrors.RefreshTokenValidationFailed;
        }

        var accessToken = tokenGenerator.GenerateAccessToken(
            user.Id,
            user.Email,
            user.Name,
            user.Roles);
        var refreshToken = tokenGenerator.GenerateRefreshToken(user.Id);

        return new AuthenticationResult(
            user.Id,
            user.Email,
            user.Name,
            accessToken,
            refreshToken);
    }
}