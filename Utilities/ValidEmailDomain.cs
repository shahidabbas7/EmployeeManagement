using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Utilities
{
    public class ValidEmailDomain:ValidationAttribute
    {
        private readonly string allowedemail;

        public ValidEmailDomain(string allowedemail)
        {
            this.allowedemail = allowedemail;
        }
        public override bool IsValid(object value)
        {
            string[] strings = value.ToString().Split("@");
            return strings[1] == allowedemail;
        }
    }
}
