using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.ViewModels
{
    public class EmployeeEditVM:EmployeeVM
    {
        public int id { get; set; }
        public string existingpath { get; set; }
    }
}
