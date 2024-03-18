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

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<AuthenticationResult>> Login(LoginRequest request)
    {
        var command = new LoginCommand(
            Email: request.Email,
            Password: request.Password);

        var result = await sender.Send(command);

        return result.Match(
            _ => Ok(result.Value),
            Error);
    }

    [AllowAnonymous]
    [HttpPost("{userId}/tokens/refresh")]
    public async Task<ActionResult<AuthenticationResult>> RefreshToken(Guid userId, RefreshTokenRequest request)
    {
        var accessToken = GetAccessToken();
        var command = new RefreshTokenCommand(
            UserId: userId,
            AccessToken: accessToken,
            RefreshToken: request.RefreshToken);

        var result = await sender.Send(command);

        return result.Match(
            _ => Ok(result.Value),
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

    private string GetAccessToken() =>
        Request.Headers.Authorization.ToString().Split(' ')[1];
}