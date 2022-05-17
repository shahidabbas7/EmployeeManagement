using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.ViewModels
{
    public class EditRolesVM
    {
        public EditRolesVM()
        {
            Users = new List<string>();
        }
        public string id { get; set; }
        public string Name { get; set; }
        public List<string> Users { get; set; }
    }
}
