
using FluentValidation;

public class DeleteDepartmentValidator : AbstractValidator<DeleteDepartmentCommand>
{
    public DeleteDepartmentValidator()
    {
        RuleFor(x=>x.id).GreaterThan(0).WithMessage("Id Must Be Greater Than 0 ");
    }
}