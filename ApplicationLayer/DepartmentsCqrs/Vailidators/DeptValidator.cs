using ApplicationLayer.DepartmentsCqrs.Commands;
using ApplicationLayer.DepartmentsCqrs.Queries;
using DomainLayer;
using FluentValidation;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.DepartmentsCqrs.Vailidators
{
    public class CreateDeptValidator : AbstractValidator<CreateDeptCommand>   
    {
        public CreateDeptValidator()
        {
            RuleFor(x => x.DeptDto.Name).NotEmpty().
                WithMessage("Dept Name Is Required ").
                MinimumLength(3).WithMessage("3 Letters Is Required At Least").
                Matches("^[a-zA-Z0-9 ]+$").WithMessage("Only Letters Numbers And Spaces");

            RuleFor(x=>x.DeptDto.Manager).NotEmpty().
                WithMessage("Manager Name Is Required").
                MinimumLength(3).WithMessage("3 Letters Is Required At Least").
                Matches("^[a-zA-Z ]+$").WithMessage("Only Letters And Spaces");

        }
    }

    public class UpdateDeptValidator : AbstractValidator<UpdateDeptCommand>
    {
        public UpdateDeptValidator()
        {
            RuleFor(x => x.DeptDto.Name).NotEmpty().
               WithMessage("Dept Name Is Required ").
               MinimumLength(3).WithMessage("3 Letters Is Required At Least").
               Matches("^[a-zA-Z0-9 ]+$").WithMessage("Only Letters Numbers And Spaces");

            RuleFor(x => x.DeptDto.Manager).NotEmpty().
                WithMessage("Manager Name Is Required").
                MinimumLength(3).WithMessage("3 Letters Is Required At Least").
                Matches("^[a-zA-Z ]+$").WithMessage("Only Letters And Spaces");
        }

    }

    public class GetByIdValidator : AbstractValidator<GetDeptByIdQuery>
    {
        public GetByIdValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id Must Be Greater Than 0 ");
        }
    }

    public class DeleteDeptValidator : AbstractValidator<DeleteDeptCommand>
    {
        public DeleteDeptValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id Must Be Greater Than 0 ");
        }
    }
}
