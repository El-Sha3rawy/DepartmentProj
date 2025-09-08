using FluentValidation;
using Microsoft.Extensions.Localization;
using Shared.Proto;
using TestDept.SharedResources;

namespace TestDept.UI_Validation
{


    public class DeptModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Manager { get; set; } = string.Empty;
    }
    public class CreateDeptValidator : AbstractValidator<DeptModel>
        {

            
        public CreateDeptValidator(IStringLocalizer<SharedResource> localizer)
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

    public class UpdateDeptValidator : AbstractValidator<DeptModel>
    {
        public UpdateDeptValidator(IStringLocalizer<SharedResource> localizer)
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
