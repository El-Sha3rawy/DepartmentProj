
using FluentValidation;

public class UpdateDepartmentValidator : AbstractValidator<UpdateDepartmentCommand>
{
    public UpdateDepartmentValidator()
    {
        RuleFor(x => x.updateCommand.Name).NotEmpty()
           .WithMessage("Dept Name Is Required")
           .MinimumLength(3).WithMessage("3 Letters Is Required At Least")
           .Matches("^[a-zA-Z0-9 ]+$").WithMessage("Only Letters Numbers And Spaces");

        RuleFor(x => x.updateCommand.Manager).NotEmpty()
            .WithMessage("Dept Name Is Required")
            .MinimumLength(3).WithMessage("3 Letters Is Required At Least")
            .Matches("^[a-zA-Z ]+$").WithMessage("Only Letters Numbers And Spaces");
    }
}