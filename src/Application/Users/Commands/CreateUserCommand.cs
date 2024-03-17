using Application.Core;

using Domain.Users;

using MediatR;

namespace Application.Users.Commands;

public record CreateUserCommand(
    string Email,
    string Password,
    string Name
) : IRequest<Result<User>>;