
using FluentValidation;

public class GetDepartmentByIdValidator : AbstractValidator<GetDepartmentByIdQuery>
{
    public GetDepartmentByIdValidator()
    {
        RuleFor(x=>x.id).GreaterThan(0).WithMessage("Id Must Be Greater Than 0");
    }
}