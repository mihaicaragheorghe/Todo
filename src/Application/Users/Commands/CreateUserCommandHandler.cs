using Application.Common.Errors;
using Application.Core;

using Domain.Users;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace Application.Users.Commands;

public class CreateUserCommandHandler(IUserRepository userRepository)
    : IRequestHandler<CreateUserCommand, Result<User>>
{
    public async Task<Result<User>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (await userRepository.FindByEmailAsync(request.Email) is not null)
        {
            return UserErrors.EmailInUse;
        }

        var user = User.Create(
            email: request.Email,
            name: request.Name);

        // todo add password hashing and user roles

        await userRepository.AddAsync(user, cancellationToken);

        return user;
    }
}