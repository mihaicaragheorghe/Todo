using FluentValidation;

namespace Application.TodoLists.Commands.ToggleTodoListArchieve;

public class ToggleTodoListArchieveCommandValidator : AbstractValidator<ToggleTodoListArchieveCommand>
{
    public ToggleTodoListArchieveCommandValidator()
    {
        RuleFor(v => v.TodoListId)
            .NotEmpty();

        RuleFor(v => v.UserId)
            .NotEmpty();
    }
}