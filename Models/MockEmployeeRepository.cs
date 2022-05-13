using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private List<Employee> emplist;
        public MockEmployeeRepository()
        {
            emplist = new List<Employee>()
          {
              new Employee{id=1,Name="Shahid",Department=dept.Development,Email="Shahid@gmail.com"},
              new Employee{id=2,Name="Bilal Shabeer",Department=dept.Development,Email="BilalShabeer@gmail.com"},
              new Employee{id=3,Name="Shahid Abbas",Department=dept.Development,Email="Shahidabbas@gmail.com"},

          };
        }

        public Employee addemployee(Employee employee)
        {
            //getting the next id
            employee.id = emplist.Max(x => x.id) + 1;
            emplist.Add(employee);
            return employee;
        }

        public Employee DeleteEmployee(int id)
        {
            Employee employee = emplist.FirstOrDefault(x=>x.id==id);
            if (employee != null)
            {
                emplist.Remove(employee);
            }
            return employee;
        }

        public IEnumerable<Employee> getallemployees()
        {
            return emplist;
        }

        public Employee GetEmployee(int id)
        {
            return emplist.FirstOrDefault(x => x.id == id);
        }

        public Employee Updateemployee(Employee employeechanges)
        {
            Employee employee = emplist.FirstOrDefault(x => x.id == employeechanges.id);
            if (employee != null)
            {
                employee.Name = employeechanges.Name;
                employee.Email = employeechanges.Email;
                employee.Department = employeechanges.Department;
            }
            return employee;
        }
    }
}
