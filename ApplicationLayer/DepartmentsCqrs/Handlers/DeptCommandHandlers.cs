using ApplicationLayer.DepartmentsCqrs.Commands;
using ApplicationLayer.DepartmentsCqrs.Queries;
using AutoMapper;
using DomainLayer;
using InfrastructureLayer;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.DepartmentsCqrs.Handlers
{
    public class DeptCommandHandlers : IRequestHandler<CreateDeptCommand, Department>,
        IRequestHandler<UpdateDeptCommand,bool>,
        IRequestHandler<DeleteDeptCommand,bool>
    {
        private readonly IRepository _Irepo;
        private readonly IMapper _mapper;

        public DeptCommandHandlers (IRepository irepo, IMapper mapper)
        {
            _Irepo = irepo;
            _mapper = mapper;
        }
        
        public async Task<Department> Handle(CreateDeptCommand command,CancellationToken cancellationToken)
        {
            var Check = await _Irepo.GetByName(command.DeptDto.Name);

            if (Check == true)
            {
                throw new ValidationException("This Name Is Already Existed");
            }
                
            var Dept = _mapper.Map<Department>(command.DeptDto);
            Dept.CreatedDate = DateOnly.FromDateTime(DateTime.UtcNow);
            var New = await _Irepo.Create(Dept);

            return New;

        }

        public async Task <bool> Handle(UpdateDeptCommand command,CancellationToken cancellationToken)
        {
            var Dept = await _Irepo.GetById(command.Id);

            if (Dept == null)
                throw new ValidationException("The Department Is not Exicted");
            

           var Response = await _Irepo.Update(command.Id,command.DeptDto);

            return Response ; 
            
        }

        public async Task <bool> Handle(DeleteDeptCommand command, CancellationToken cancellationToken)
        {
            var Dept = await _Irepo.GetById(command.Id);
            
            if (Dept == null) 
            throw new ValidationException("The Department Is not Exicted");
            
           var Response = await _Irepo.Delete(command.Id);
            return Response ;

        }

       
    }
}
