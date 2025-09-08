using DomainLayer;
using FluentResults;
using MediatR;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.DepartmentsCqrs.Commands
{
    public record CreateDeptCommand (DepartmentDto DeptDto): IRequest<Department>; 
    public record UpdateDeptCommand (int Id , DepartmentDto DeptDto) : IRequest <bool>;

    public record DeleteDeptCommand (int Id) : IRequest<bool>;
}
