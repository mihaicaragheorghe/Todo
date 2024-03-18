using Application.Users.Commands;
using Application.Users.Contracts;
using Application.Users.Queries;

using Domain.Users;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class UsersController(ISender sender) : ApiController
{
    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<UserDto>> Create(CreateUserRequest request)
    {
        var command = new CreateUserCommand(
            Email: request.Email,
            Name: request.Name,
            Password: request.Password);

        var result = await sender.Send(command);

        return result.Match(
            user => CreatedAtAction(
                actionName: nameof(GetById),
                routeValues: new { id = user.Id },
                value: MapToDto(user)),
            Error);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetById(Guid id)
    {
        var result = await sender.Send(new GetUserByIdQuery(id));

        if (result is null)
        {
            return NotFound();
        }

        return result.Match(
            user => Ok(MapToDto(user)),
            Error);
    }

    private UserDto MapToDto(User user) =>
        new(Id: user.Id,
            Email: user.Email,
            Name: user.Name,
            ProfilePictureUrl: user.ProfilePictureUrl);
}