using DomainLayer;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.DepartmentsCqrs.Queries
{
    public record GetDeptByIdQuery (int Id ) : IRequest<Department?>;
    public record GetAllDeptQuery () : IRequest<IEnumerable<Department>>;
}
