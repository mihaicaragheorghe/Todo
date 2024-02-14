using Domain.TodoLists;

using FluentValidation;

namespace Application.TodoLists.Commands.CreateTodoList;

public class CreateTodoListCommandValidator : AbstractValidator<CreateTodoListCommand>
{
    public CreateTodoListCommandValidator()
    {
        RuleFor(v => v.Title)
            .MaximumLength(TodoListConstants.NameMaxLength)
            .WithMessage($"Title must not exceed {TodoListConstants.NameMaxLength} characters.")
            .NotEmpty()
            .WithMessage("Title cannot be empty.");

        RuleFor(v => v.UserId)
            .NotEmpty()
            .WithMessage("UserId cannot be empty.");
    }
}