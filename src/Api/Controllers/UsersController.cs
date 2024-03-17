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
    public async Task<ActionResult<User>> Create(CreateUserRequest request)
    {
        var command = new CreateUserCommand(
            Email: request.Email,
            Name: request.Name,
            Password: request.Password);

        return Ok(await sender.Send(command));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetById(Guid id)
    {
        return Ok(await sender.Send(new GetUserByIdQuery(id)));
    }
}