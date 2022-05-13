using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
   public interface IEmployeeRepository
    {
        Employee GetEmployee(int id);
        IEnumerable<Employee> getallemployees();
        Employee addemployee(Employee employee);
        Employee Updateemployee(Employee employeechanges);
        Employee DeleteEmployee(int id);
    }
}
