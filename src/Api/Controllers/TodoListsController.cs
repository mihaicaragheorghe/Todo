using Api.Contracts.Common;
using Api.Contracts.TodoLists;

using Application.Common.Errors;
using Application.Core;
using Application.TodoLists.Commands.CreateTodoList;
using Application.TodoLists.Commands.DeleteTodoList;
using Application.TodoLists.Commands.RenameTodoList;
using Application.TodoLists.Commands.ReorderTodoLists;
using Application.TodoLists.Commands.ToggleTodoListArchieve;
using Application.TodoLists.Contracts;
using Application.TodoLists.Queries.GetTodoList;
using Application.TodoLists.Queries.GetTodoLists;
using Application.Todos.Contracts;

using Domain.TodoLists;
using Domain.Todos;

using Infrastructure.Security.CurrentUser;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class TodoListsController(
    ISender sender,
    ICurrentUserProvider currentUserProvider)
        : ApiController
{
    [HttpGet]
    public async Task<ActionResult<List<TodoListDto>>> GetTodoLists(CancellationToken cancellationToken)
    {
        var currentUser = GetRequiredCurrentUser();
        var query = new GetTodoListsQuery(currentUser.UserId);
        var result = await sender.Send(query, cancellationToken);

        return result.Match(
            lists => Ok(lists.ConvertAll(ToDto)),
            Error);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoListDto>> GetTodoList(Guid id, CancellationToken cancellationToken)
    {
        var currentUser = GetRequiredCurrentUser();
        var query = new GetTodoListQuery(id, currentUser.UserId);
        var result = await sender.Send(query, cancellationToken);

        return result.Match(
            list => Ok(ToDto(list)),
            Error);
    }

    [HttpPost]
    public async Task<ActionResult<TodoListDto>> Create(
        CreateTodoListRequest request,
        CancellationToken cancellationToken)
    {
        var currentUser = GetRequiredCurrentUser();
        var command = new CreateTodoListCommand(currentUser.UserId, request.Title);
        var result = await sender.Send(command, cancellationToken);

        return result.Match(
            list => CreatedAtAction(
                actionName: nameof(GetTodoList),
                routeValues: new { id = list.Id },
                value: ToDto(list)),
            Error);
    }

    [HttpPost("{id}/name/{newName}")]
    public async Task<ActionResult> Rename(Guid id, string newName, CancellationToken cancellationToken)
    {
        var currentUser = GetRequiredCurrentUser();
        var command = new RenameTodoListCommand(id, currentUser.UserId, newName);
        var result = await sender.Send(command, cancellationToken);

        return result.Match(
            _ => NoContent(),
            Error);
    }

    [HttpPost("reorder")]
    public async Task<ActionResult> Reorder(
        ReorderTodoListRequest request,
        CancellationToken cancellationToken)
    {
        var currentUser = GetRequiredCurrentUser();
        var command = new ReorderTodoListsCommand(request.Items, currentUser.UserId);
        var result = await sender.Send(command, cancellationToken);

        return result.Match(
            _ => NoContent(),
            Error);
    }

    [HttpPost("{id}/archive")]
    public async Task<ActionResult> Archive(Guid id, CancellationToken cancellationToken)
    {
        var currentUser = GetRequiredCurrentUser();
        var command = new ToggleTodoListArchieveCommand(id, currentUser.UserId, true);
        var result = await sender.Send(command, cancellationToken);

        return result.Match(
            _ => NoContent(),
            Error);
    }

    [HttpPost("{id}/unarchive")]
    public async Task<ActionResult> Unarchive(Guid id, CancellationToken cancellationToken)
    {
        var currentUser = GetRequiredCurrentUser();
        var command = new ToggleTodoListArchieveCommand(id, currentUser.UserId, false);
        var result = await sender.Send(command, cancellationToken);

        return result.Match(
            _ => NoContent(),
            Error);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var currentUser = GetRequiredCurrentUser();
        var command = new DeleteTodoListCommand(id, currentUser.UserId);
        var result = await sender.Send(command, cancellationToken);

        return result.Match(
            _ => NoContent(),
            Error);
    }

    private CurrentUser GetRequiredCurrentUser() =>
        currentUserProvider.GetCurrentUser()
            ?? throw new ServiceException(UserErrors.Unauthorized);

    private TodoListDto ToDto(TodoList todoList) =>
        new(todoList.Id,
            todoList.UserId,
            todoList.Name,
            todoList.Order,
            todoList.IsArchived,
            todoList.CreatedAtUtc,
            todoList.Todos.Select(ToDto).ToList());

    private TodoDto ToDto(Todo todo) =>
        new(todo.Id,
            todo.UserId,
            todo.ListId,
            todo.CreatedAtUtc,
            todo.Title,
            todo.Description,
            todo.DueDateUtc,
            todo.DueTimeUtc,
            todo.IsCompleted);
}