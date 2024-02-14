using Application.Core;

using Domain.TodoLists;

using MediatR;

namespace Application.TodoLists.Commands.CreateTodoList;

public class CreateTodoListCommandHandler(ITodoListRepository todoListRepository)
    : IRequestHandler<CreateTodoListCommand, Result<TodoList>>
{
    public async Task<Result<TodoList>> Handle(CreateTodoListCommand command, CancellationToken cancellationToken)
    {
        var todoList = new TodoList(command.UserId, command.Title);

        await todoListRepository.AddAsync(todoList, cancellationToken);

        return todoList;
    }
}