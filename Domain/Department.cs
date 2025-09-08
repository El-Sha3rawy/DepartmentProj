using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public  string Manager { get; set; }
        public DateOnly CreatedDate { get; set; }
        public virtual DateTime? LastUpdate {  get; set; } =null;
    }
}
