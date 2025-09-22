
using FluentValidation;

public class CreateDepartmentValidator : AbstractValidator<CreateDepartmentCommand>
{
    public CreateDepartmentValidator()
    {
        RuleFor(x => x.createCommand.Name).NotEmpty()
            .WithMessage("Dept Name Is Required")
            .MinimumLength(3).WithMessage("3 Letters Is Required At Least")
            .Matches("^[a-zA-Z0-9 ]+$").WithMessage("Only Letters Numbers And Spaces");

        RuleFor(x => x.createCommand.Manager).NotEmpty()
            .WithMessage("Dept Name Is Required")
            .MinimumLength(3).WithMessage("3 Letters Is Required At Least")
            .Matches("^[a-zA-Z ]+$").WithMessage("Only Letters Numbers And Spaces");
    }
}