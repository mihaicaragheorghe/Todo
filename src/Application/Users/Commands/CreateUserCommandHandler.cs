using Application.Common.Errors;
using Application.Core;

using Domain.Users;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace Application.Users.Commands;

public class CreateUserCommandHandler(IUserRepository userRepository, IPasswordHasher<User> passwordHasher)
    : IRequestHandler<CreateUserCommand, Result<User>>
{
    public async Task<Result<User>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (await userRepository.FindByEmailAsync(request.Email, cancellationToken) is not null)
        {
            return UserErrors.EmailInUse;
        }

        var user = User.Create(
            email: request.Email,
            name: request.Name);

        user.SetPasswordHash(passwordHasher.HashPassword(user, request.Password));

        await userRepository.AddAsync(user, cancellationToken);

        return user;
    }
}