using Application.Core;

using Domain.Users;

using MediatR;

namespace Application.Users.Queries;

public record GetUserByIdQuery(Guid Id) : IRequest<Result<User>>;