using Application.Common.Errors;
using Application.Core;

using Domain.Users;

using MediatR;

namespace Application.Users.Queries;

public class GetUserByIdQueryHandler(IUserRepository userRepository)
    : IRequestHandler<GetUserByIdQuery, Result<User>>
{
    public async Task<Result<User>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.Id, cancellationToken);

        if (user is null)
        {
            return UserErrors.NotFound;
        }

        return user;
    }
}