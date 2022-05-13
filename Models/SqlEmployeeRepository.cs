using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class SqlEmployeeRepository : IEmployeeRepository
    {
        private readonly contextdb _dbcontext;

        public SqlEmployeeRepository(contextdb db)
        {
            _dbcontext = db;
        }
        public Employee addemployee(Employee employee)
        {
            _dbcontext.employees.Add(employee);
            _dbcontext.SaveChanges();
            return employee;
        }

        public Employee DeleteEmployee(int id)
        {
            Employee employee = _dbcontext.employees.FirstOrDefault(x => x.id == id);
            if (employee != null)
            {
                _dbcontext.employees.Remove(employee);
                _dbcontext.SaveChanges();
            }
            return employee;
        }

        public IEnumerable<Employee> getallemployees()
        {
            return _dbcontext.employees;
        }

        public Employee GetEmployee(int id)
        {
            Employee employee = _dbcontext.employees.Find(id);
            return employee;
        }

        public Employee Updateemployee(Employee employeechanges)
        {
            var employee = _dbcontext.employees.Attach(employeechanges);
            employee.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _dbcontext.SaveChanges();
            return employeechanges;
        }
    }
}
