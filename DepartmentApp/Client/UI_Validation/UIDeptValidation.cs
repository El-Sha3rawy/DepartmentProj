using FluentValidation;
using Microsoft.Extensions.Localization;
using DepartmentGrpc.Proto;
using TestDept.SharedResources;

namespace TestDept.UI_Validation
{


    public class DeptModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Manager { get; set; } = string.Empty;
    }
    public class CreateDepartmentValidator : AbstractValidator<DeptModel>
        {

            
        public CreateDepartmentValidator(IStringLocalizer<SharedResource> localizer)
            {
                RuleFor(x => x.Name).NotEmpty().
                    WithMessage(localizer["RequiredField"]).
                    MinimumLength(3).WithMessage(localizer["MinLength"]).
                    Matches("^[a-zA-Z0-9 ]+$").WithMessage(localizer["OnlyLettersNumbers"]);

                RuleFor(x => x.Manager).NotEmpty().
                    WithMessage(localizer["RequiredField"]).
                    MinimumLength(3).WithMessage(localizer["MinLength"]).
                    Matches("^[a-zA-Z ]+$").WithMessage(localizer["OnlyLetters"]);

            }
        }

    public class UpdateDepartmentValidator : AbstractValidator<DeptModel>
    {
        public UpdateDepartmentValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(x => x.Name).NotEmpty().
               WithMessage(localizer["RequiredField"]).
               MinimumLength(3).WithMessage(localizer["MinLength"]).
               Matches("^[a-zA-Z0-9 ]+$").WithMessage(localizer["OnlyLettersNumbers"]);

            RuleFor(x => x.Manager).NotEmpty().
                WithMessage(localizer["RequiredField"]).
                MinimumLength(3).WithMessage(localizer["MinLength"]).
                Matches("^[a-zA-Z ]+$").WithMessage(localizer["OnlyLetters"]);
        }

    }

}
