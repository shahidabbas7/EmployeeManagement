using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class contextdb:DbContext
    {
        public contextdb(DbContextOptions<contextdb> options):base(options)
        {
                
        }
        public DbSet<Employee> employees { get; set; }
    }
}
