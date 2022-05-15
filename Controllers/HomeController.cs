using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeerepository;
        private readonly IHostingEnvironment hostingEnvironment;

        public HomeController(IEmployeeRepository employeeRepository, IHostingEnvironment hostingEnvironment)
        {
            _employeerepository = employeeRepository;
            this.hostingEnvironment = hostingEnvironment;
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
            Employee employee = _employeerepository.GetEmployee(id.Value);
            if (employee == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", id.Value);
            }
            HomeDetialsVM homeDetialsVM = new HomeDetialsVM()
            {
                employee =employee,
                Title = "Employee Detials"

            };
            return View(homeDetialsVM);
        }
        public ActionResult create()
        {
            return View();
        } 
        [HttpPost]
        public ActionResult create(EmployeeVM newemployee)
        {
            //check if model state is valid
            if(ModelState.IsValid)
            {
                string uniquefilenaame = fileuploadmethod(newemployee);
               
                Employee employee = new Employee()
                {
                    Name = newemployee.Name,
                    Email = newemployee.Email,
                    Department = newemployee.Department,
                    photopath = uniquefilenaame
                };
                //calling addemployee method
                _employeerepository.addemployee(employee);
                return RedirectToAction("detials", new { id = employee.id });
            }
            return View(newemployee);
        }
        public ActionResult edit(int id)
        {
            Employee employee = _employeerepository.GetEmployee(id);
            EmployeeEditVM employeeEditVM = new EmployeeEditVM()
            {
                id = employee.id,
                Name = employee.Name,
                Department = employee.Department,
                Email = employee.Email,
                existingpath = employee.photopath
            };
            return View(employeeEditVM);
        } 
        [HttpPost]
        public ActionResult edit(EmployeeEditVM model)
        {
            //check if model state is valid
            if (ModelState.IsValid)
            {
                Employee employee = _employeerepository.GetEmployee(model.id);
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Department = model.Department;
                if (model.photo != null)
                {
                    if (model.existingpath != null)
                    {
                        string photopath = Path.Combine(hostingEnvironment.WebRootPath, "images", model.existingpath);
                        System.IO.File.Delete(photopath);
                    }
                    employee.photopath = fileuploadmethod(model);

                }

                //calling addemployee method
                _employeerepository.Updateemployee(employee);
                return RedirectToAction("index");
            }
            return View(model);

        }

        private string fileuploadmethod(EmployeeVM newemployee)
        {
            string uniquefilenaame = null;
            if (newemployee.photo != null)
            {
                
                //geting root directory path i.e ~/wwwwroot/images
                string filepath = Path.Combine(hostingEnvironment.WebRootPath, "images");
                //generating new unique file name using guid that will be stored in database
                uniquefilenaame = Guid.NewGuid().ToString() + "_" + newemployee.photo.FileName;
                //combining the file paths to generate a file path that will be uploaded on the server
                string fileuploadpath = Path.Combine(filepath, uniquefilenaame);
                //uploading the file on the server using copyto method
                using(var filestream= new FileStream(fileuploadpath, FileMode.Create))
                {
                   newemployee.photo.CopyTo(filestream);
                }
               
            }

            return uniquefilenaame;
        }
    }
}
