using Application.Common.Errors;
using Application.Common.Interfaces;
using Application.Core;
using Application.Users.Contracts;

using Domain.Users;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace Application.Users.Commands;

public class LoginCommandHandler(
    IUserRepository userRepository,
    ITokenGenerator tokenGenerator,
    IPasswordHasher<User> passwordHasher)
        : IRequestHandler<LoginCommand, Result<AuthenticationResult>>
{
    public async Task<Result<AuthenticationResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindByEmailAsync(request.Email, cancellationToken);

        if (user is null)
        {
            return UserErrors.InvalidCredentials;
        }

        var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

        if (result == PasswordVerificationResult.Failed)
        {
            return UserErrors.InvalidCredentials;
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