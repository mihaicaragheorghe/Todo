using Domain.TodoLists;

using FluentValidation;

namespace Application.TodoLists.Commands.RenameTodoList;

public class RenameTodoListCommandValidator : AbstractValidator<RenameTodoListCommand>
{
    public RenameTodoListCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty()
            .WithMessage("Id cannot be empty.");

        RuleFor(v => v.Name)
            .MaximumLength(TodoListConstants.NameMaxLength)
            .WithMessage($"Title must not exceed {TodoListConstants.NameMaxLength} characters.")
            .NotEmpty()
            .WithMessage("Title cannot be empty.");
    }
}