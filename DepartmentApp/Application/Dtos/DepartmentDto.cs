using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
    public class DepartmentDto
    {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Manager { get; set; }
    public DateOnly CreatedDate { get; set; }
    public virtual DateTime? LastUpdate { get; set; } 
}

