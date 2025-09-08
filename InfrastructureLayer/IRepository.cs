using DomainLayer;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer
{
    public interface IRepository
    {
        Task<Department> GetById(int id);
        Task <bool> GetByName(string name);
        Task<IEnumerable<Department>> GetAll();
        Task<Department> Create(Department department);
        Task <bool> Update (int id , DepartmentDto departmentDto);
        Task <bool> Delete (int id);
    }
}
