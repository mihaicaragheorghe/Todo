using Application.Core;
using Application.Users.Contracts;

using MediatR;

namespace Application.Users.Commands;

public record RefreshTokenCommand(
    Guid UserId,
    string AccessToken,
    string RefreshToken
) : IRequest<Result<AuthenticationResult>>;