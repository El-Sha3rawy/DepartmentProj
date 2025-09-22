using Infrastructure;
using Microsoft.EntityFrameworkCore;

public class DepartmentRepository : IDepartmentRepository
{

    private readonly AppDbContext _context;
    public DepartmentRepository (AppDbContext context)
    {
        _context = context;
    }
    public async Task CreateDepartment(Department department)
    {
        _context.Departments.Add(department);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteDepartment(int id)
    {
        var department = await GetDepartmentById(id);
        if (department != null)
        {
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
        
    }

    public async Task<IEnumerable<Department>> GetAllDepartments()
    {
        return await _context.Departments.ToListAsync();
    }

    public async Task<Department> GetDepartmentById(int id)
    {
       var department = await _context.Departments.FindAsync(id);
         
       if (department != null)
        {
            return department;
        }
       throw new KeyNotFoundException();
    }

    public async Task<bool> GetDepartmentByName(int id, string name)
    {
        if (id == 0)
        {
            return await _context.Departments.AnyAsync(x => x.Name == name);
        }
        return await _context.Departments.AnyAsync(x => x.Name == name && x.Id!=id);
    }

    public async Task<bool> UpdateDepartment(Department department)
    {
        var Dept = await _context.Departments.FindAsync(department.Id);
        Dept.LastUpdate = DateTime.Now;
        Dept.Name = department.Name;
        Dept.Manager = department.Manager;
        await _context.SaveChangesAsync();
        return true;
    }
} 