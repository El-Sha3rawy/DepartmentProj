using AutoMapper;
using DomainLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer
{
    public class Repository : IRepository
    {
        private readonly AppDbContext _Context;
        private readonly IMapper _mapper;

        public Repository (AppDbContext context,IMapper mapper)
        {
            _Context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Department>> GetAll()
        {
            return await _Context.departmenrs.ToListAsync();
        }

        public async Task<Department> GetById(int id)
        {
            return await _Context.departmenrs.FindAsync(id);
        }

        public async Task <bool> GetByName(string Name)
        {
           return await _Context.departmenrs.AnyAsync(x => x.Name == Name);
            
            

        }

        public async Task<Department> Create(Department depatrment)
        {
            _Context.departmenrs.Add(depatrment);
            await _Context.SaveChangesAsync();
            return depatrment;
        }

        public async Task <bool> Update (int id , DepartmentDto departmentDto)
        {
            var dept = await _Context.departmenrs.FindAsync(id);
             
            _mapper.Map(departmentDto,dept);
            dept.LastUpdate = DateTime.Now ;
            
            await _Context.SaveChangesAsync();

            return true;
        }

        public async Task <bool> Delete(int id)
        {
            var department = await GetById(id);

            if (department!= null)
            {
                _Context.departmenrs.Remove(department);
                await _Context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
