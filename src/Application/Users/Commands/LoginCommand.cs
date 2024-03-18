using Application.Core;
using Application.Users.Contracts;

using MediatR;

namespace Application.Users.Commands;

public record LoginCommand(
    string Email,
    string Password
) : IRequest<Result<AuthenticationResult>>;