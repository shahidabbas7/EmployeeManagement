using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public AdminController(RoleManager<IdentityRole> roleManager,UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        public IActionResult CreateRole()
        {

            return View();
        }
    [HttpPost]
    public async Task<IActionResult> CreateRole(CreateRoleVM model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole()
                {
                    Name = model.RoleName
                };
              IdentityResult result= await roleManager.CreateAsync(identityRole);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "admin");
                }
                else
                {
                    foreach(IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

            }
            return View(model);
        }
        public async Task<IActionResult> EditRoles(string id)
        {
            var role =await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"role with id:{id} can not be found";
                return View("NotFound");
            }
            var model = new EditRolesVM()
            {
                id = role.Id,
                Name = role.Name
            };
            foreach(var user in userManager.Users)
            {
              if(await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }
            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> EditRoles(EditRolesVM model)
        {
            var role = await roleManager.FindByIdAsync(model.id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"role with id:{model.id} can not be found";
                return View("NotFound");
            }
            role.Name = model.Name;
           var result= await roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("ListRoles");
            }
            else
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
            

        }
        public async Task<IActionResult> EditUsersInrole(string roleid)
        {
            ViewBag.roleid = roleid;
            var role = await roleManager.FindByIdAsync(roleid);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"role with id:{roleid} can not be found";
                return View("NotFound");
            }
            var model = new List<UserRolesVM>();
            foreach (var user in userManager.Users)
            {
                var userrole = new UserRolesVM
                {
                    Userid = user.Id,
                    username = user.UserName
                };
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userrole.IsSelected = true;
                }
                else
                {
                    userrole.IsSelected = false;
                }
                model.Add(userrole);
            }
            return View(model);


        }
        [HttpPost]
        public async Task<IActionResult> EditUsersInrole(List<UserRolesVM> model,string roleid)
        {
       
            var role = await roleManager.FindByIdAsync(roleid);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"role with id:{roleid} can not be found";
                return View("NotFound");
            }
           for(int i=0; i<model.Count; i++)
            {
              var user= await  userManager.FindByIdAsync(model[i].Userid);
                IdentityResult result = null;
                if(model[i].IsSelected&& !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                if(!model[i].IsSelected&& (await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }
                if (result.Succeeded)
                {
                    if (i < (model.Count-1))
                        continue;
                    else
                    return RedirectToAction("EditRoles", new { id = roleid });
                }
            }
            return RedirectToAction("EditRoles", new { id = roleid });
        }

            public IActionResult ListRoles()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }
    }
}
