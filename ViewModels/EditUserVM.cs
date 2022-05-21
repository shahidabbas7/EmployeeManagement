using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.ViewModels
{
    public class EditUserVM
    {
        public EditUserVM()
        {
            Roles = new List<string>();
            claims = new List<string>();
        }
        public string id { get; set; }
        [Required]
        public string username { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }
        public String City { get; set; }

        public List<string> Roles { get; set; }
        public List<string> claims { get; set; }
    }
}
