using EmployeeManagement.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.ViewModels
{
    public class RegisterVM
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Remote(action:"Isemailvalid",controller:"account")]
        [ValidEmailDomain(allowedemail:"gmail.com",ErrorMessage ="email domain must be gmail.com")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }
        [Required]
        [Display(Name ="Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("password",ErrorMessage ="password does not match")]
        public string Confirmpassword { get; set; }
        public string City { get; set; }
    }
}
