using ApplicationLayer.DepartmentsCqrs.Queries;
using AutoMapper;
using DomainLayer;
using InfrastructureLayer;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.DepartmentsCqrs.Handlers
{
    public class DeptQueryHandler : IRequestHandler<GetDeptByIdQuery, Department>,
       IRequestHandler<GetAllDeptQuery, IEnumerable<Department>>
    {

        private readonly IRepository _Irepo; 

        public DeptQueryHandler (IRepository Irepo)
        {
            _Irepo = Irepo;
            
        }
        public async Task<Department> Handle(GetDeptByIdQuery query,CancellationToken cancellationToken)
        {
            var Dept = await _Irepo.GetById(query.Id);

            if (Dept == null)
                throw new ValidationException("This Department Is not Found");

            return Dept;
        }

       public async Task<IEnumerable<Department>> Handle (GetAllDeptQuery query,CancellationToken cancellationToken)
        {
           IEnumerable<Department> Depts = await _Irepo.GetAll();
           
           return Depts; 

        }
    }
}
