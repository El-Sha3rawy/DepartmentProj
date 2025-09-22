public interface IDepartmentRepository
{
    Task<Department> GetDepartmentById(int id);
    Task<bool> GetDepartmentByName(int id ,string name);
    Task CreateDepartment(Department department);
    Task <bool>UpdateDepartment(Department department);
    Task <bool>DeleteDepartment(int id);
    Task<IEnumerable<Department>> GetAllDepartments(); 
}