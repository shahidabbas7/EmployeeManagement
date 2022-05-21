using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<AdminController> logger;
        //admin constructor
        public AdminController(RoleManager<IdentityRole> roleManager,UserManager<ApplicationUser> userManager
            ,ILogger<AdminController> logger)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.logger = logger;
        }
        //get//admin/createrole
        public IActionResult CreateRole()
        {

            return View();
        }
    [HttpPost]
    //post/admin/createroles
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
        //get/admin/Editroles
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
        //post/admin/Editrples
        public async Task<IActionResult> EditRoles(EditRolesVM model)
        {
            var role = await roleManager.FindByIdAsync(model.id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"role with id:{model.id} can not be found";
                return View("NotFound");
            }

            role.Name = model.Name;
            var result = await roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("ListRoles");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);



            }
        }
        //get/admin/EditUsersInrole
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
        //post/admin/EditUsersInrole
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
        //get/admin/Manageroles
        public async Task<IActionResult> Manageroles(string userid)
        {
            ViewBag.userid = userid;
            var user = await userManager.FindByIdAsync(userid);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"role with id:{userid} can not be found";
                return View("NotFound");
            }
            var model = new List<RolesUserVM>();
            foreach(var roles in roleManager.Roles)
            {
                var userRoles = new RolesUserVM
                {
                    Roleid = roles.Id,
                    Rolename = roles.Name
                };
                if(await userManager.IsInRoleAsync(user, roles.Name))
                {
                    userRoles.IsSelected = true;
                }
                else
                {
                    userRoles.IsSelected = false;
                }
                model.Add(userRoles);
            }
            return View(model);
        }
        [HttpPost]
        //get/admin/Manageroles
        public async Task<IActionResult> Manageroles(List<RolesUserVM> model,string userid)
        {
            var user = await userManager.FindByIdAsync(userid);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"role with id:{userid} can not be found";
                return View("NotFound");
            }
            for(int i = 0; i < model.Count; i++)
            {
                IdentityResult result = null;
                if(model[i].IsSelected&&!(await userManager.IsInRoleAsync(user, model[i].Rolename)))
                {
                   result= await userManager.AddToRoleAsync(user,model[i].Rolename);
                }
                if (!model[i].IsSelected && (await userManager.IsInRoleAsync(user, model[i].Rolename)))
                {
                   result= await userManager.RemoveFromRoleAsync(user, model[i].Rolename);
                }
                else
                {
                    continue;
                }
                if (result.Succeeded)
                {
                    if (i < model.Count - 1)
                    {
                        continue;
                    }
                    else
                        return RedirectToAction("ListUsers", new { id = userid });
                }
                
            }
            return RedirectToAction("ListUsers", new { id = userid });

        }
        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }
        public IActionResult ListUsers()
        {
           var user= userManager.Users;
            return View(user);
        }
        public async Task<IActionResult> EditUsers(string id)
        {
            var user =await userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"role with id:{id} can not be found";
                return View("NotFound");
            }
            var userroles = await userManager.GetRolesAsync(user);
            var claims = await userManager.GetClaimsAsync(user);
            EditUserVM model = new EditUserVM()
            {
                id = user.Id,
                username = user.UserName,
                email = user.Email,
                City=user.City,
                claims = claims.Select(x => x.Value).ToList(),
                Roles = userroles.ToList()
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditUsers(EditUserVM model)
        {
            var user = await userManager.FindByIdAsync(model.id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"role with id:{model.id} can not be found";
                return View("NotFound");
            }
            user.UserName = model.username;
            user.Email = model.email;
            user.City = model.City;
            var result = await userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("ListUsers");
            }
            foreach(var error in result.Errors)
            {
             ModelState.AddModelError("", error.Description);
            }
            
            return View(model);
        }
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id:{id} can not be found";
                return View("NotFound");
            }
            var result = await userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("ListUsers");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return RedirectToAction("ListUsers");
        }
        public async Task<IActionResult> DeleteRoles(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"role with id:{id} can not be found";
                return View("NotFound");
            }
            else
            {
                try
                {
                    var result = await roleManager.DeleteAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListRoles");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return RedirectToAction("ListRoles");
                }
                catch (DbUpdateException ex)
                {
                    logger.LogError($"exception occur:{ex}");
                    ViewBag.Title = $"{role.Name} role is in use";
                    ViewBag.message = $"{role.Name} can not be deleted as there are Users in this role" +
                        $" if you want to delete the role delete the users first";
                    return View("error");
                }
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
