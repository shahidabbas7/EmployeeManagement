using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeerepository;

        public HomeController(IEmployeeRepository employeeRepository)
        {
            _employeerepository = employeeRepository;
        }
        //Get /home /Index
       
        public ViewResult Index()
        {
           var model=_employeerepository.getallemployees();
            return View(model);
        }
        //Get /home /Detials
        public ViewResult Detials(int? id)
        {
            HomeDetialsVM homeDetialsVM = new HomeDetialsVM()
            {
                employee = _employeerepository.GetEmployee(id??1),
                Title = "Employee Detials"

            };
            return View(homeDetialsVM);
        }
        public ActionResult create()
        {
            return View();
        } 
        [HttpPost]
        public ActionResult create(Employee employee)
        {
            //check if model state is valid
            if(ModelState.IsValid)
            {
                //calling addemployee method
                _employeerepository.addemployee(employee);
                return RedirectToAction("detials", new { id = employee.id });
            }
            return View(employee);
        }
    }
}
