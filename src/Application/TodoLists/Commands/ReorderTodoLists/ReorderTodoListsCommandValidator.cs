using FluentValidation;

namespace Application.TodoLists.Commands.ReorderTodoLists;

public class ReorderTodoListsCommandValidator : AbstractValidator<ReorderTodoListsCommand>
{
    public ReorderTodoListsCommandValidator()
    {
        RuleFor(v => v.ItemOrders).NotEmpty();

        RuleFor(v => v.UserId).NotEmpty();
    }
}
